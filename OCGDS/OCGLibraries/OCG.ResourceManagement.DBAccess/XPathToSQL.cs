using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OCG.ResourceManagement.DBAccess
{
    #region Using Memory Table with Check-Rights-Switch

    #region AttributeClass

    public class AttributeSchema
    {
        public string Key;
        public string Name;
        public string Type;
    }

    #endregion

    public class XPathToSQL
    {
        #region Patterns

        private string patternReplaceBooleanTrue = @"\s*=\s*true";
        private string patternReplaceBooleanFalse = @"\s*=\s*false";
        private string patternNotExpression = @"not\(.*=.*\)";
        private string patternValidXPath = @"^/(\w+)|\*\[(?>\((?<DEPTH>)|\)(?<-DEPTH>)|(\[|\]|\\|/)(?<SC>)|.?)*(?(DEPTH)(?!))(?(SC)(?!))\](/\w+)?$";
        private string patternAttributeName = @"(\(\s*\w+\s*,)|(\[\s*\w+\s*=)|(\s*\w+\s*=)|(\[\s*\w+\s*\>)|(\s*\w+\s*\>)|(\[\s*\w+\s*\<)|(\s*\w+\s*\<)|(\[\s*\w+\s*\>=)|(\s*\w+\s*\>=)|(\[\s*\w+\s*\<=)|(\s*\w+\s*\<=)";
        private string patternFunction = @"(starts-with|ends-with|contains)\(\s*\w+\s*,\s*'(\w+|%|\.|,|-|_|\s)+'\s*\)";
        private string patternReference = @"(^/\w+|\*)|(/\w+$)";
        private string patternReferenceEqualOpt = @"{0}\s*=\s*#";

        #endregion

        #region SQL Queries

        #region Union Table

        private string unionTable = string.Empty;

        private string queryUnionTable = @"
        select
		    *
	    from
		    [{0}] with (nolock)
        ";

        #endregion

        #region Get Attribute Schema

        private string queryGetAttributes = @"
        select distinct
	        [Key],
	        [Name],
	        [DataType]
        from
	        [fim].[AttributeInternal] as [ai] with (nolock)
        where
	        [ai].[Name] in ({0})
        ";

        #endregion

        #region Get searched Objects

        private string queryGetSearchedObjects = @"

        declare @_rs{5} table ([ObjectKey] bigint, [ObjectTypeKey] smallint, [ObjectID] uniqueidentifier, [DisplayName] nvarchar(448))
        declare @rs{5} table ([ObjectKey] bigint, [ObjectID] uniqueidentifier)
        
        insert into @_rs{5}
        select
	        [o].[ObjectKey],
	        [o].[ObjectTypeKey],
	        [o].[ObjectID],
            [strDisplayName].[ValueString]
        from
            [fim].[Objects] as [o] with (nolock)
        full join
            [fim].[ObjectValueString] as [strDisplayName] with (nolock)
        on
	        [strDisplayName].[ObjectKey] = [o].[ObjectKey] and [strDisplayName].[AttributeKey] = 66
        {1}
        where
		    [o].[ObjectKey] is not null {4}
		    and
		    ({2})
        order by
            [strDisplayName].[ValueString]
        {6}

        insert into @rs{5}
        select
	        [o].[ObjectKey],
	        [o].[ObjectID]
        from
	        @_rs{5} as [o]

        ";

        private string queryCheckObjectRights = @"
        where
            [o].[ObjectKey] IN
			(
				SELECT [smr].[ValueReference]
				FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
				WHERE
						[smr].[ObjectTypeKey] = 29 /*Set*/
					AND	[smr].[AttributeKey] = 40 /*ComputedMember*/
					AND	[smr].[ObjectKey] IN
					(
						SELECT [mpr].[ResourceCurrentSet]
						FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
						WHERE
								[mpr].[PrincipalSet] IN
								(
									SELECT [smP].[ObjectKey]
									FROM [fim].[ObjectValueReference] AS [smP] with (nolock)
									WHERE
											[smP].[ObjectTypeKey] = 29 /*Set*/
										AND	[smP].[AttributeKey] = 40 /*ComputedMember*/
										AND	[smP].[ValueReference] = (select [ObjectKey] from [fim].[Objects] with (nolock) where [ObjectID] = '{0}')
								)
								AND
								(
									[mpr].[ActionParameterAll] = 1
									OR
									[mpr].[ObjectKey] IN
									(
										SELECT [mpra].[ObjectKey]
										FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
										WHERE
												[mpra].[ObjectKey] = [mpr].[ObjectKey]
											AND	[mpra].[ActionParameterKey] = 66 /*DisplayName*/
									)
								)
					)
			)
			OR 
            [o].[ObjectKey] IN
			(
				SELECT [smR].[ValueReference]
				FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
				WHERE
						[smR].[ObjectTypeKey] = 29 /*Set*/
					AND	[smR].[AttributeKey] = 40 /*ComputedMember*/
					AND [smR].[ObjectKey] IN
					(
						SELECT [mpr].[ResourceCurrentSet]
						FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
						WHERE
								[mpr].[PrincipalRelativeToResource]	IN
								(
									SELECT [ovrR].[AttributeKey]
									FROM [fim].[ObjectValueReference] AS [ovrR] with (nolock)
									WHERE
											[ovrR].[ObjectKey] = [o].[ObjectKey]
										AND	[ovrR].[ObjectTypeKey] = [o].[ObjectTypeKey]
										AND	[ovrR].[ValueReference] = (select [ObjectKey] from [fim].[Objects] with (nolock) where [ObjectID] = '{0}')
								)
								AND
								(
									[mpr].[ActionParameterAll] = 1
									OR
									[mpr].[ObjectKey] IN
									(
										SELECT [mpra].[ObjectKey]
										FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
										WHERE
												[mpra].[ObjectKey] = [mpr].[ObjectKey]
											AND	[mpra].[ActionParameterKey] = 66 /*DisplayName*/
									)
								)
					)
			)
        ";

        #endregion

        #region Get referenced Objects

        private string queryGetReferencedObjects = @"
        
        declare @_rrs{1} table ([ObjectKey] bigint, [ObjectTypeKey] smallint, [ObjectID] uniqueidentifier)
        declare @rrs{1} table ([ObjectKey] bigint, [ObjectID] uniqueidentifier)

        insert into @_rrs{1}
        select distinct
	        [o].[ObjectKey],
            [o].[ObjectTypeKey],
	        [o].[ObjectID]
        from
	        [fim].[ObjectValueReference] as [r] with (nolock)
        right join
	        [fim].[Objects] as [o] with (nolock)
        on
	        [o].[ObjectKey] = [r].[ValueReference]
        where
	        [r].AttributeKey = {0}
	        and
	        [r].[ObjectKey] in (select [ObjectKey] from @rs{1})

        insert into @rrs{1}
        select 
            [o].[ObjectKey],
	        [o].[ObjectID]
        from
            @_rrs{1} as [o]

        ";

        string queryCheckReferencedObjectRights = @"
        where
            [o].[ObjectKey] IN
			(
				SELECT [smr].[ValueReference]
				FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
				WHERE
						[smr].[ObjectTypeKey] = 29 /*Set*/
					AND	[smr].[AttributeKey] = 40 /*ComputedMember*/
					AND	[smr].[ObjectKey] IN
					(
						SELECT [mpr].[ResourceCurrentSet]
						FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
						WHERE
								[mpr].[PrincipalSet] IN
								(
									SELECT [smP].[ObjectKey]
									FROM [fim].[ObjectValueReference] AS [smP] with (nolock)
									WHERE
											[smP].[ObjectTypeKey] = 29 /*Set*/
										AND	[smP].[AttributeKey] = 40 /*ComputedMember*/
										AND	[smP].[ValueReference] = (select [ObjectKey] from [fim].[Objects] with (nolock) where [ObjectID] = '{2}')
								)
								AND
								(
									[mpr].[ActionParameterAll] = 1
									OR
									[mpr].[ObjectKey] IN
									(
										SELECT [mpra].[ObjectKey]
										FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
										WHERE
												[mpra].[ObjectKey] = [mpr].[ObjectKey]
											AND	[mpra].[ActionParameterKey] = 66 /*DisplayName*/
									)
								)
					)
			)
			OR 
            [o].[ObjectKey] IN
			(
				SELECT [smR].[ValueReference]
				FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
				WHERE
						[smR].[ObjectTypeKey] = 29 /*Set*/
					AND	[smR].[AttributeKey] = 40 /*ComputedMember*/
					AND [smR].[ObjectKey] IN
					(
						SELECT [mpr].[ResourceCurrentSet]
						FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
						WHERE
								[mpr].[PrincipalRelativeToResource]	IN
								(
									SELECT [ovrR].[AttributeKey]
									FROM [fim].[ObjectValueReference] AS [ovrR] with (nolock)
									WHERE
											[ovrR].[ObjectKey] = [o].[ObjectKey]
										AND	[ovrR].[ObjectTypeKey] = [o].[ObjectTypeKey]
										AND	[ovrR].[ValueReference] = (select [ObjectKey] from [fim].[Objects] with (nolock) where [ObjectID] = '{2}')
								)
								AND
								(
									[mpr].[ActionParameterAll] = 1
									OR
									[mpr].[ObjectKey] IN
									(
										SELECT [mpra].[ObjectKey]
										FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
										WHERE
												[mpra].[ObjectKey] = [mpr].[ObjectKey]
											AND	[mpra].[ActionParameterKey] = 66 /*DisplayName*/
									)
								)
					)
			)

        ";

        #endregion

        #region Get Attributes to load

        private string queryGetAttributesToLoad = @"
        [{0}] ([ObjectKey], [ObjectID], [AttributeKey], [ObjectTypeKey], [Value])	-- get value
        as
        (
	        select
		        [{5}].[ObjectKey],
		        [{5}].[ObjectID],
		        [{1}].[AttributeKey],
		        [{1}].[ObjectTypeKey],
		        cast([{1}].[{2}] as varchar(max))
	        from
		        @{5} as [{5}]
	        left join
		        [fim].[{3}] as [{1}] with (nolock)
	        on
		        [{5}].[ObjectKey] = [{1}].[ObjectKey]
	        where
		        [{1}].[AttributeKey] in ({4})
                
        ";

        private string queryGetBinaryAttributesToLoad = @"
        [{0}] ([ObjectKey], [ObjectID], [AttributeKey], [ObjectTypeKey], [Value])	-- get value
        as
        (
	        select
		        [{5}].[ObjectKey],
		        [{5}].[ObjectID],
		        [{1}].[AttributeKey],
		        [{1}].[ObjectTypeKey],
                CAST(N'' AS XML).value('xs:base64Binary(xs:hexBinary(sql:column(""[{1}].[{2}]"")))', 'VARCHAR(MAX)')
	        from
		        @{5} as [{5}]
	        left join
		        [fim].[{3}] as [{1}] with (nolock)
	        on
		        [{5}].[ObjectKey] = [{1}].[ObjectKey]
	        where
		        [{1}].[AttributeKey] in ({4})
                
        ";

        private string queryCheckAttributeRights = @"
        and	-- Policy constraints on requested attributes start
		(
			[{1}].[ObjectKey] IN
			(
				SELECT [smr].[ValueReference]
				FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
				WHERE
						[smr].[ObjectTypeKey] = 29 /*Set*/
					AND	[smr].[AttributeKey] = 40 /*ComputedMember*/
					AND	[smr].[ObjectKey] IN
					(
						SELECT [mpr].[ResourceCurrentSet]
						FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
						WHERE
								[mpr].[PrincipalSet] IN
								(
									SELECT [smP].[ObjectKey]
									FROM [fim].[ObjectValueReference] AS [smP] with (nolock)
									WHERE
											[smP].[ObjectTypeKey] = 29 /*Set*/
										AND	[smP].[AttributeKey] = 40 /*ComputedMember*/
										AND	[smP].[ValueReference] = (select [ObjectKey] from [fim].[Objects] with (nolock) where [ObjectID] = '{7}')
								)
								AND
								(
									[mpr].[ActionParameterAll] = 1
									OR
									[mpr].[ObjectKey] IN
									(
										SELECT [mpra].[ObjectKey]
										FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
										WHERE
												[mpra].[ObjectKey] = [mpr].[ObjectKey]
											AND	[mpra].[ActionParameterKey] = [{1}].[AttributeKey] 
									)
								)
					)
			)
			OR
			(
				[{1}].[ObjectKey] IN
				(
					SELECT [smR].[ValueReference]
					FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
					WHERE
							[smR].[ObjectTypeKey] = 29 /*Set*/
						AND	[smR].[AttributeKey] = 40 /*ComputedMember*/
						AND [smR].[ObjectKey] IN
						(
							SELECT [mpr].[ResourceCurrentSet]
							FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
							WHERE
									[mpr].[PrincipalRelativeToResource]	IN
									(
										SELECT [ovrR].[AttributeKey]
										FROM [fim].[ObjectValueReference] AS [ovrR] with (nolock)
										WHERE
												[ovrR].[ObjectKey] = [{1}].[ObjectKey]
											AND	[ovrR].[ObjectTypeKey] = [{1}].[ObjectTypeKey]
											AND	[ovrR].[ValueReference] = (select [ObjectKey] from [fim].[Objects] with (nolock) where [ObjectID] = '{7}')
									)
									AND
									(
										[mpr].[ActionParameterAll] = 1
										OR
										[mpr].[ObjectKey] IN
										(
											SELECT [mpra].[ObjectKey]
											FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
											WHERE
													[mpra].[ObjectKey] = [mpr].[ObjectKey]
												AND	[mpra].[ActionParameterKey] = [{1}].[AttributeKey] 
										)
									)
						)
				)
			)
		)	-- Policy constraints on requested attributes end
        ";

        #endregion

        #region Get Reference Attributes to load

        private string queryGetReferenceAttributesToLoad = @"
        [{0}] ([ObjectKey], [ObjectID], [AttributeKey], [ObjectTypeKey], [Value])	-- get value
        as
        (
	        select
		        [{5}].[ObjectKey],
		        [{5}].[ObjectID],
		        [{1}].[AttributeKey],
		        [{1}].[ObjectTypeKey],
		        cast([{6}].[{7}] as varchar(max))
	        from
		        @{5} as [{5}]
	        left join
		        [fim].[{3}] as [{1}] with (nolock)
	        on
		        [{5}].[ObjectKey] = [{1}].[ObjectKey]
            left join
				[fim].[Objects] as [{6}] with (nolock)
			on
				[{1}].[{2}] = [{6}].[ObjectKey]
	        where
		        [{1}].[AttributeKey] in ({4})
		      
        ";

        private string queryCheckReferenceAttributeRight = @"
        and	-- Policy constraints on requested attributes start
		(
			[{1}].[ObjectKey] IN
			(
				SELECT [smr].[ValueReference]
				FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
				WHERE
						[smr].[ObjectTypeKey] = 29 /*Set*/
					AND	[smr].[AttributeKey] = 40 /*ComputedMember*/
					AND	[smr].[ObjectKey] IN
					(
						SELECT [mpr].[ResourceCurrentSet]
						FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
						WHERE
								[mpr].[PrincipalSet] IN
								(
									SELECT [smP].[ObjectKey]
									FROM [fim].[ObjectValueReference] AS [smP] with (nolock)
									WHERE
											[smP].[ObjectTypeKey] = 29 /*Set*/
										AND	[smP].[AttributeKey] = 40 /*ComputedMember*/
										AND	[smP].[ValueReference] = (select [ObjectKey] from [fim].[Objects] with (nolock) where [ObjectID] = '{9}')
								)
								AND
								(
									[mpr].[ActionParameterAll] = 1
									OR
									[mpr].[ObjectKey] IN
									(
										SELECT [mpra].[ObjectKey]
										FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
										WHERE
												[mpra].[ObjectKey] = [mpr].[ObjectKey]
											AND	[mpra].[ActionParameterKey] = [{1}].[AttributeKey] 
									)
								)
					)
			)
			OR
			(
				[{1}].[ObjectKey] IN
				(
					SELECT [smR].[ValueReference]
					FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
					WHERE
							[smR].[ObjectTypeKey] = 29 /*Set*/
						AND	[smR].[AttributeKey] = 40 /*ComputedMember*/
						AND [smR].[ObjectKey] IN
						(
							SELECT [mpr].[ResourceCurrentSet]
							FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
							WHERE
									[mpr].[PrincipalRelativeToResource]	IN
									(
										SELECT [ovrR].[AttributeKey]
										FROM [fim].[ObjectValueReference] AS [ovrR] with (nolock)
										WHERE
												[ovrR].[ObjectKey] = [{1}].[ObjectKey]
											AND	[ovrR].[ObjectTypeKey] = [{1}].[ObjectTypeKey]
											AND	[ovrR].[ValueReference] = (select [ObjectKey] from [fim].[Objects] with (nolock) where [ObjectID] = '{9}')
									)
									AND
									(
										[mpr].[ActionParameterAll] = 1
										OR
										[mpr].[ObjectKey] IN
										(
											SELECT [mpra].[ObjectKey]
											FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
											WHERE
													[mpra].[ObjectKey] = [mpr].[ObjectKey]
												AND	[mpra].[ActionParameterKey] = [{1}].[AttributeKey] 
										)
									)
						)
				)
			)
		)	-- Policy constraints on requested attributes end
        ";

        #endregion

        #region Get Final Result

        private string queryGetFinalResult = @"
        select {1}
	        [frs].[ObjectID],
	        [ObjectType].[Name] as ObjectType,
	        [binding].[AttributeName],
            [attribute].[DataType] as [AttributeType],
	        [frs].[Value] as [AttributeValue],
	        [ovString].[ValueString] as [DisplayName],
	        [attribute].[Multivalued]
        from
	        [fim].[BindingInternal] as [binding] with (nolock)
        right join
        (
	        {0}
        ) as [frs]
        on
	        [binding].[ObjectTypeKey] = [frs].[ObjectTypeKey]
	        and
	        [binding].[AttributeKey] = [frs].[AttributeKey]
        left join
	        [fim].[ObjectValueString] as [ovString] with (nolock)
        on
	        [ovString].[AttributeKey] = 66
	        and
	        [ovString].[ObjectKey] = [frs].[ObjectKey]
	        and
	        [ovString].[LocaleKey] = 127
        left join
	        [fim].[AttributeInternal] as [attribute] with (nolock)
        on
	        [attribute].[Key] = [frs].[AttributeKey]
        left join
	        [fim].[ObjectTypeInternal] as [objecttype] with (nolock)
        on
	        [objecttype].[Key] = [frs].[ObjectTypeKey]
    ";

        #endregion

        #endregion

        #region Help methods

        private void parseAttribute()
        {
            foreach (Match match in Regex.Matches(this.XPathQuery, patternAttributeName, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase))
            {
                string value = match.Value.Trim();

                if (value.StartsWith("[") || value.StartsWith("("))
                {
                    value = value.Substring(1);
                }

                if (value.EndsWith(">=") || value.EndsWith("<="))
                {
                    value = value.Substring(0, value.Length - 2);
                }
                if (value.EndsWith("=") || value.EndsWith(",") || value.EndsWith(">") || value.EndsWith("<"))
                {
                    value = value.Substring(0, value.Length - 1);
                }

                if (this.AttributeList.FirstOrDefault(a => a.Equals(value, StringComparison.OrdinalIgnoreCase)) == null)
                {
                    this.AttributeList.Add(value);
                }
            }
        }

        private void parseFunction()
        {
            foreach (Match match in Regex.Matches(this.XPathQuery, patternFunction, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase))
            {
                string value = match.Value.Trim();

                if (this.FunctionList.FirstOrDefault(f => f.Equals(value, StringComparison.OrdinalIgnoreCase)) == null)
                {
                    this.FunctionList.Add(value);
                }
            }
        }

        private void parseReference()
        {
            foreach (Match match in Regex.Matches(this.XPathQuery, patternReference, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase))
            {
                string value = match.Value.Trim();

                if (value.StartsWith("/"))
                {
                    value = value.Substring(1);
                }

                if (this.ReferenceList.FirstOrDefault(r => r.Equals(value, StringComparison.OrdinalIgnoreCase)) == null)
                {
                    this.ReferenceList.Add(value);
                }
            }
        }

        private void getSearchCriteria()
        {
            int beginPos = this.XPathQuery.IndexOf("[");
            int endPos = this.XPathQuery.LastIndexOf("]");

            this.SearchCriteria = this.XPathQuery.Substring(beginPos + 1, endPos - beginPos - 1);

            foreach (string function in this.FunctionList)
            {
                string value = string.Empty;
                string[] values = function.Split("'".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (values.Length == 3)
                {
                    value = values[1].Trim();
                }

                string[] opts = function.Split(new string[] { "(", ",", ")" }, StringSplitOptions.RemoveEmptyEntries);

                if (opts.Length < 3)
                {
                    continue;
                }

                string con = string.Empty;

                if (opts.Length == 3)
                {
                    value = opts[2].Trim();
                    if (value.StartsWith("'") && value.EndsWith("'"))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }
                    else
                    {
                        continue;
                    }
                }

                switch (opts[0].Trim().ToLower())
                {
                    case "contains":
                        con = string.Format("({0} like '%{1}%')", opts[1].Trim(), value);
                        break;
                    case "starts-with":
                        con = string.Format("({0} like '{1}%')", opts[1].Trim(), value);
                        break;
                    case "ends-with":
                        con = string.Format("({0} like '%{1}')", opts[1].Trim(), value);
                        break;
                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(con))
                {
                    this.SearchCriteria = this.SearchCriteria.Replace(function, con);
                }
            }
        }

        private void getSQLQuerySerachedObjects(int level, bool checkRights = true, int count = -1, int skip = 0)
        {
            string conString = string.Empty;
            string conDateTime = string.Empty;
            string conBoolean = string.Empty;
            string conReference = string.Empty;
            string conInteger = string.Empty;
            string conBinary = string.Empty;

            int cntString = 1;
            int cntDateTime = 1;
            int cntBoolean = 1;
            int cntReference = 1;
            int cntInteger = 1;
            int cntBinary = 1;

            foreach (AttributeSchema att in this.SchemaList)
            {
                switch (att.Type.ToLower())
                {
                    case "string":

                        conString = string.Format(
                            @"  {0}
                            full join
	                            [fim].[ObjectValueString] as [string{1}] with (nolock)
                            on
	                            [string{1}].[ObjectKey] = [o].[ObjectKey] and [string{1}].[AttributeKey] = {2}
                        ",
                            conString, cntString, att.Key);

                        this.SearchCriteria = this.SearchCriteria.Replace(att.Name, string.Format("[string{0}].[ValueString]", cntString));

                        cntString++;

                        break;
                    case "integer":

                        conInteger = string.Format(
                            @"  {0}
                            full join
	                            [fim].[ObjectValueInteger] as [integer{1}] with (nolock)
                            on
	                            [integer{1}].[ObjectKey] = [o].[ObjectKey] and [integer{1}].[AttributeKey] = {2}
                        ",
                            conInteger, cntInteger, att.Key);

                        this.SearchCriteria = this.SearchCriteria.Replace(att.Name, string.Format("[integer{0}].[ValueInteger]", cntInteger));

                        cntString++;

                        break;
                    case "reference":

                        conReference = string.Format(
                            @"  {0}
                            full join
                                [fim].[ObjectValueReference] as [reference{1}] with (nolock)
                            on
                                [reference{1}].[AttributeKey] = {2} and [o].[ObjectKey] = [reference{1}].[ObjectKey]
                            full join
	                            [fim].[ObjectValueIdentifier] as [identifier{1}] with (nolock)
                            on
	                            [identifier{1}].[ObjectKey] = [reference{1}].[ValueReference]
                        ",
                            conReference, cntReference, att.Key);


                        foreach (Match match in Regex.Matches(this.SearchCriteria, string.Format(this.patternReferenceEqualOpt, att.Name)))
                        {
                            this.SearchCriteria = this.SearchCriteria.Replace(match.ToString(), string.Format("{0} in #", att.Name));
                        }
                        this.SearchCriteria = this.SearchCriteria.Replace(att.Name, string.Format("[identifier{0}].[ValueIdentifier]", cntReference));

                        cntReference++;

                        break;
                    case "datetime":

                        conDateTime = string.Format(
                            @"  {0}
                            full join
	                            [fim].[ObjectValueDateTime] as [datetime{1}] with (nolock)
                            on
	                            [datetime{1}].[ObjectKey] = [o].[ObjectKey] and [datetime{1}].[AttributeKey] = {2}
                        ",
                            conDateTime, cntDateTime, att.Key);

                        this.SearchCriteria = this.SearchCriteria.Replace(att.Name, string.Format("[datetime{0}].[ValueDateTime]", cntDateTime));

                        cntDateTime++;

                        break;
                    case "boolean":

                        conBoolean = string.Format(
                            @"  {0}
                            full join
	                            [fim].[ObjectValueBoolean] as [boolean{1}] with (nolock)
                            on
	                            [boolean{1}].[ObjectKey] = [o].[ObjectKey] and [boolean{1}].[AttributeKey] = {2}
                        ",
                            conBoolean, cntBoolean, att.Key);

                        this.SearchCriteria = this.SearchCriteria.Replace(att.Name, string.Format("[boolean{0}].[ValueBoolean]", cntBoolean));

                        cntBoolean++;

                        break;
                    case "binary":

                        conBoolean = string.Format(
                            @"  {0}
                            full join
	                            [fim].[ObjectValueBinary] as [binary{1}] with (nolock)
                            on
	                            [binary{1}].[ObjectKey] = [o].[ObjectKey] and [binary{1}].[AttributeKey] = {2}
                        ",
                            conBinary, cntBinary, att.Key);

                        this.SearchCriteria = this.SearchCriteria.Replace(att.Name, string.Format("[binary{0}].[ValueBinary]", cntBinary));

                        cntBinary++;

                        break;
                    default:
                        break;
                }
            }

            string joins = string.Format("{0} \n{1} \n{2} \n{3} \n{4} \n{5}", conString, conBoolean, conReference, conInteger, conDateTime, conBinary);

            string limit = count < 0 ? string.Empty : string.Format("offset {0} rows fetch next {1} rows only", skip, count);

            if (!checkRights)
            {
                this.SQLQuery = string.Format(this.queryGetSearchedObjects,
                    this.ActorID, joins, this.SearchCriteria,
                    "PlaceHolder",
                    this.SearchObjectType.Equals("*") ? string.Empty : string.Format("and [o].[ObjectTypeKey] = (select [Key] from [fim].[ObjectTypeInternal] where [Name] = '{0}')", this.SearchObjectType),
                    level, limit);
            }
            else
            {
                this.SQLQuery = string.Format(this.queryGetSearchedObjects + this.queryCheckObjectRights,
                    this.ActorID, joins, this.SearchCriteria,
                    "PlaceHolder",
                    this.SearchObjectType.Equals("*") ? string.Empty : string.Format("and [o].[ObjectTypeKey] = (select [Key] from [fim].[ObjectTypeInternal] where [Name] = '{0}')", this.SearchObjectType),
                    level, limit);
            }


        }

        private void getSQLQueryReferencedObjects(int level, bool checkRights)
        {
            if (this.HasReference)
            {
                AttributeSchema att = this.SchemaList.FirstOrDefault(s => s.Name.Equals(this.ReferenceList.Last()));

                if (att != null)
                {
                    if (!checkRights)
                    {
                        this.SQLQuery = string.Format("{0}{1}",
                            this.SQLQuery, string.Format(this.queryGetReferencedObjects, att.Key, level, this.ActorID));
                    }
                    else
                    {
                        this.SQLQuery = string.Format("{0}{1}", this.SQLQuery,
                            string.Format(this.queryGetReferencedObjects + this.queryCheckReferencedObjectRights, att.Key, level, this.ActorID));
                    }
                }
            }
        }

        private void getSQLQueryAttributesToLoad(int level, bool checkRights)
        {
            // String Attributes
            string attString = string.Empty;
            foreach (AttributeSchema att in this.SchemaAttributesToLoad.Where(s => s.Type.Equals("string", StringComparison.OrdinalIgnoreCase)))
            {
                attString = string.IsNullOrEmpty(attString) ? att.Key : string.Format("{0},{1}", attString, att.Key);
            }
            if (!string.IsNullOrEmpty(attString))
            {
                string attributeQuery;
                if (!checkRights)
                {
                    attributeQuery = string.Format(this.queryGetAttributesToLoad + ")",
                        "StringValue", "ovString", "ValueString", "ObjectValueString", attString,
                        this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
                        level, this.ActorID);
                }
                else
                {
                    attributeQuery = string.Format(this.queryGetAttributesToLoad + this.queryCheckAttributeRights + ")",
                        "StringValue", "ovString", "ValueString", "ObjectValueString", attString,
                        this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
                        level, this.ActorID);
                }

                if (SQLQuery.IndexOf(";with", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    attributeQuery = string.Format(";with {0}", attributeQuery);
                }
                else
                {
                    attributeQuery = string.Format(", {0}", attributeQuery);
                }
                this.SQLQuery = string.Format("{0}{1}",
                    SQLQuery,
                    attributeQuery);

                this.unionTable = string.IsNullOrEmpty(this.unionTable) ?
                    string.Format(this.queryUnionTable, "StringValue") :
                    string.Format("{0}union{1}", this.unionTable, string.Format(this.queryUnionTable, "StringValue"));
            }

            // DateTime Attributes
            string attDateTime = string.Empty;
            foreach (AttributeSchema att in this.SchemaAttributesToLoad.Where(s => s.Type.Equals("datetime", StringComparison.OrdinalIgnoreCase)))
            {
                attDateTime = string.IsNullOrEmpty(attDateTime) ? att.Key : string.Format("{0},{1}", attDateTime, att.Key);
            }
            if (!string.IsNullOrEmpty(attDateTime))
            {
                string attributeQuery;
                if (!checkRights)
                {
                    attributeQuery = string.Format(this.queryGetAttributesToLoad + ")",
                        "DateTimeValue", "ovDateTime", "ValueDateTime", "ObjectValueDateTime", attDateTime,
                        this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
                        level, this.ActorID);
                }
                else
                {
                    attributeQuery = string.Format(this.queryGetAttributesToLoad + this.queryCheckAttributeRights + ")",
                        "DateTimeValue", "ovDateTime", "ValueDateTime", "ObjectValueDateTime", attDateTime,
                        this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
                        level, this.ActorID);
                }

                if (SQLQuery.IndexOf(";with", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    attributeQuery = string.Format(";with {0}", attributeQuery);
                }
                else
                {
                    attributeQuery = string.Format(", {0}", attributeQuery);
                }
                this.SQLQuery = string.Format("{0}{1}",
                    SQLQuery,
                    attributeQuery);

                this.unionTable = string.IsNullOrEmpty(this.unionTable) ?
                    string.Format(this.queryUnionTable, "DateTimeValue") :
                    string.Format("{0}union{1}", this.unionTable, string.Format(this.queryUnionTable, "DateTimeValue"));
            }

            // Boolean Attributes
            string attBoolean = string.Empty;
            foreach (AttributeSchema att in this.SchemaAttributesToLoad.Where(s => s.Type.Equals("boolean", StringComparison.OrdinalIgnoreCase)))
            {
                attBoolean = string.IsNullOrEmpty(attBoolean) ? att.Key : string.Format("{0},{1}", attBoolean, att.Key);
            }
            if (!string.IsNullOrEmpty(attBoolean))
            {
                string attributeQuery;
                if (!checkRights)
                {
                    attributeQuery = string.Format(this.queryGetAttributesToLoad + ")",
                        "BooleanValue", "ovBoolean", "ValueBoolean", "ObjectValueBoolean", attBoolean,
                        this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
                        level, this.ActorID);
                }
                else
                {
                    attributeQuery = string.Format(this.queryGetAttributesToLoad + this.queryCheckAttributeRights + ")",
                        "BooleanValue", "ovBoolean", "ValueBoolean", "ObjectValueBoolean", attBoolean,
                        this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
                        level, this.ActorID);
                }

                if (SQLQuery.IndexOf(";with", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    attributeQuery = string.Format(";with {0}", attributeQuery);
                }
                else
                {
                    attributeQuery = string.Format(", {0}", attributeQuery);
                }
                this.SQLQuery = string.Format("{0}{1}",
                    SQLQuery,
                    attributeQuery);

                this.unionTable = string.IsNullOrEmpty(this.unionTable) ?
                    string.Format(this.queryUnionTable, "BooleanValue") :
                    string.Format("{0}union{1}", this.unionTable, string.Format(this.queryUnionTable, "BooleanValue"));
            }

            // Integer Attributes
            string attInteger = string.Empty;
            foreach (AttributeSchema att in this.SchemaAttributesToLoad.Where(s => s.Type.Equals("integer", StringComparison.OrdinalIgnoreCase)))
            {
                attInteger = string.IsNullOrEmpty(attInteger) ? att.Key : string.Format("{0},{1}", attInteger, att.Key);
            }
            if (!string.IsNullOrEmpty(attInteger))
            {
                string attributeQuery;
                if (!checkRights)
                {
                    attributeQuery = string.Format(this.queryGetAttributesToLoad + ")",
                        "IntegerValue", "ovInteger", "ValueInteger", "ObjectValueInteger", attInteger,
                        this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
                        level, this.ActorID);
                }
                else
                {
                    attributeQuery = string.Format(this.queryGetAttributesToLoad + this.queryCheckAttributeRights + ")",
                        "IntegerValue", "ovInteger", "ValueInteger", "ObjectValueInteger", attInteger,
                        this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
                        level, this.ActorID);
                }

                if (SQLQuery.IndexOf(";with", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    attributeQuery = string.Format(";with {0}", attributeQuery);
                }
                else
                {
                    attributeQuery = string.Format(", {0}", attributeQuery);
                }
                this.SQLQuery = string.Format("{0}{1}",
                    SQLQuery,
                    attributeQuery);

                this.unionTable = string.IsNullOrEmpty(this.unionTable) ?
                    string.Format(this.queryUnionTable, "IntegerValue") :
                    string.Format("{0}union{1}", this.unionTable, string.Format(this.queryUnionTable, "IntegerValue"));
            }

            // Reference Attributes
            string attReference = string.Empty;
            foreach (AttributeSchema att in this.SchemaAttributesToLoad.Where(s => s.Type.Equals("reference", StringComparison.OrdinalIgnoreCase)))
            {
                attReference = string.IsNullOrEmpty(attReference) ? att.Key : string.Format("{0},{1}", attReference, att.Key);
            }
            if (!string.IsNullOrEmpty(attReference))
            {
                string attributeQuery;
                if (!checkRights)
                {
                    attributeQuery = string.Format(this.queryGetReferenceAttributesToLoad + ")",
                        "IdentifierValue", "ovReference", "ValueReference", "ObjectValueReference",
                        attReference, this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
                        "ovIdentity", "ObjectID", level, this.ActorID);
                }
                else
                {
                    attributeQuery = string.Format(this.queryGetReferenceAttributesToLoad + this.queryCheckReferenceAttributeRight + ")",
                        "IdentifierValue", "ovReference", "ValueReference", "ObjectValueReference",
                        attReference, this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
                        "ovIdentity", "ObjectID", level, this.ActorID);
                }

                if (SQLQuery.IndexOf(";with", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    attributeQuery = string.Format(";with {0}", attributeQuery);
                }
                else
                {
                    attributeQuery = string.Format(", {0}", attributeQuery);
                }
                this.SQLQuery = string.Format("{0}{1}",
                    SQLQuery,
                    attributeQuery);

                this.unionTable = string.IsNullOrEmpty(this.unionTable) ?
                    string.Format(this.queryUnionTable, "IdentifierValue") :
                    string.Format("{0}union{1}", this.unionTable, string.Format(this.queryUnionTable, "IdentifierValue"));
            }

            // Binary Attributes
            string attBinary = string.Empty;
            foreach (AttributeSchema att in this.SchemaAttributesToLoad.Where(s => s.Type.Equals("binary", StringComparison.OrdinalIgnoreCase)))
            {
                attBinary = string.IsNullOrEmpty(attBinary) ? att.Key : string.Format("{0},{1}", attBinary, att.Key);
            }
            if (!string.IsNullOrEmpty(attBinary))
            {
                string attributeQuery;
                if (!checkRights)
                {
                    attributeQuery = string.Format(this.queryGetBinaryAttributesToLoad + ")",
                        "BinaryValue", "ovBinary", "ValueBinary", "ObjectValueBinary", attBinary,
                        this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
                        level, this.ActorID);
                }
                else
                {
                    attributeQuery = string.Format(this.queryGetBinaryAttributesToLoad + this.queryCheckAttributeRights + ")",
                        "BinaryValue", "ovBinary", "ValueBinary", "ObjectValueBinary", attBinary,
                        this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
                        level, this.ActorID);
                }

                if (SQLQuery.IndexOf(";with", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    attributeQuery = string.Format(";with {0}", attributeQuery);
                }
                else
                {
                    attributeQuery = string.Format(", {0}", attributeQuery);
                }
                this.SQLQuery = string.Format("{0}{1}",
                    SQLQuery,
                    attributeQuery);

                this.unionTable = string.IsNullOrEmpty(this.unionTable) ?
                    string.Format(this.queryUnionTable, "BinaryValue") :
                    string.Format("{0}union{1}", this.unionTable, string.Format(this.queryUnionTable, "BinaryValue"));
            }
        }

        private void getSQLQueryFinalResult()
        {
            this.SQLQuery = string.Format("{0}{1}", this.SQLQuery, string.Format(this.queryGetFinalResult, this.unionTable, string.Empty));
        }

        #endregion

        #region Public Properties

        public string XPathQuery = string.Empty;

        public SqlConnection SQLConnection = null;

        public string ActorID = string.Empty;

        public bool IsValid = false;

        public string SearchObjectType = string.Empty;

        public bool HasReference = false;

        public List<string> AttributeList = new List<string>();

        public List<string> AttributesToLoad = new List<string>();

        public List<string> ReferenceList = new List<string>();

        public List<string> FunctionList = new List<string>();

        public string SearchCriteria = string.Empty;

        public List<AttributeSchema> SchemaList = new List<AttributeSchema>();

        public List<AttributeSchema> SchemaAttributesToLoad = new List<AttributeSchema>();

        public string SQLQuery = string.Empty;

        #endregion

        #region Constructor

        public XPathToSQL(string xpathQuery, string[] attributesToLoad, string actorID, SqlConnection conn)
        {
            this.XPathQuery = Regex.Replace(xpathQuery, patternReplaceBooleanTrue, "=1", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
            this.XPathQuery = Regex.Replace(this.XPathQuery, patternReplaceBooleanFalse, "=0", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

            foreach (Match match in Regex.Matches(this.XPathQuery, patternNotExpression, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase))
            {
                int posBegin = match.Value.IndexOf("(");
                int posEnd = match.Value.IndexOf("=");

                string attr = match.Value.Substring(posBegin + 1, posEnd - posBegin - 1);

                string replacement = string.Format("(({0} is null) or {1})", attr, match.Value);

                this.XPathQuery = this.XPathQuery.Replace(match.Value, replacement);
            }

            if (Regex.IsMatch(this.XPathQuery, patternValidXPath, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase) &&
                !string.IsNullOrEmpty(actorID) && conn != null)
            {
                this.IsValid = true;

                parseAttribute();
                parseFunction();
                parseReference();

                this.SearchObjectType = this.ReferenceList[0];

                if (this.ReferenceList.Count == 2)
                {
                    this.HasReference = true;
                }

                getSearchCriteria();

                this.AttributesToLoad = attributesToLoad.ToList();

                this.ActorID = actorID;

                this.SQLConnection = conn;
            }
        }

        #endregion

        #region Public Method

        public void GetAttributeSchema()
        {
            if (!this.IsValid)
            {
                return;
            }

            // get Search Attributes
            string attributeString = string.Empty;
            foreach (string att in this.AttributeList)
            {
                if (string.IsNullOrEmpty(attributeString))
                {
                    attributeString = string.Format("'{0}'", att);
                }
                else
                {
                    attributeString = string.Format("{0},'{1}'", attributeString, att);
                }
            }

            if (this.HasReference)
            {
                attributeString = string.Format("{0},'{1}'", attributeString, this.ReferenceList.Last());
            }

            SqlCommand cmd = new SqlCommand(string.Format(this.queryGetAttributes, attributeString), this.SQLConnection);

            SqlDataReader readerAttributeList = cmd.ExecuteReader();

            cmd.Dispose();

            while (readerAttributeList.Read())
            {
                this.SchemaList.Add(new AttributeSchema()
                {
                    Key = readerAttributeList["Key"].ToString(),
                    Name = readerAttributeList["Name"].ToString(),
                    Type = readerAttributeList["DataType"].ToString()
                });
            }

            readerAttributeList.Close();
            readerAttributeList.Dispose();


            // Get Attributes to load
            attributeString = string.Empty;
            foreach (string att in this.AttributesToLoad)
            {
                if (string.IsNullOrEmpty(attributeString))
                {
                    attributeString = string.Format("'{0}'", att);
                }
                else
                {
                    attributeString = string.Format("{0},'{1}'", attributeString, att);
                }
            }

            cmd = new SqlCommand(string.Format(this.queryGetAttributes, attributeString), this.SQLConnection);

            SqlDataReader readerAttributesToLoad = cmd.ExecuteReader();

            cmd.Dispose();

            while (readerAttributesToLoad.Read())
            {
                this.SchemaAttributesToLoad.Add(new AttributeSchema()
                {
                    Key = readerAttributesToLoad["Key"].ToString(),
                    Name = readerAttributesToLoad["Name"].ToString(),
                    Type = readerAttributesToLoad["DataType"].ToString()
                });
            }

            readerAttributesToLoad.Close();
            readerAttributesToLoad.Dispose();
        }

        public void GetSQLQuery(int level = 0, bool isEndResult = true, bool checkRights = true, int count = -1, int skip = 0)
        {
            getSQLQuerySerachedObjects(level, checkRights, count, skip);

            getSQLQueryReferencedObjects(level, checkRights);

            if (isEndResult)
            {
                getSQLQueryAttributesToLoad(level, checkRights);

                getSQLQueryFinalResult();
            }
            //else
            //{
            //    this.SQLQuery = string.Format("{0} select [ObjectKey] from [{1}{2}]", this.SQLQuery, this.HasReference ? "rrs" : "rs", level);
            //}
        }

        public string GetSubstiutionQuery(int level)
        {
            return string.Format("select [ObjectID] from @{0}{1}", this.HasReference ? "rrs" : "rs", level);
        }

        #endregion

    }

    #endregion


    #region Using WITH Statement -- Obsoleted

    //    #region AttributeClass

    //    public class AttributeSchema
    //    {
    //        public string Key;
    //        public string Name;
    //        public string Type;
    //    }

    //    #endregion

    //    public class XPathToSQL
    //    {
    //        #region Patterns

    //        private string patternReplaceBooleanTrue = @"\s*=\s*true";
    //        private string patternReplaceBooleanFalse = @"\s*=\s*false";
    //        private string patternNotExpression = @"not\(.*=.*\)";
    //        private string patternValidXPath = @"^/(\w+)|\*\[(?>\((?<DEPTH>)|\)(?<-DEPTH>)|(\[|\]|\\|/)(?<SC>)|.?)*(?(DEPTH)(?!))(?(SC)(?!))\](/\w+)?$";
    //        private string patternAttributeName = @"(\(\s*\w+\s*,)|(\[\s*\w+\s*=)|(\s*\w+\s*=)";
    //        private string patternFunction = @"(starts-with|ends-with|contains)\(\s*\w+\s*,\s*'(\w+|%|\.|-|_|\s)+'\s*\)";
    //        private string patternReference = @"(^/\w+|\*)|(/\w+$)";
    //        private string patternReferenceEqualOpt = @"{0}\s*=\s*#";

    //        #endregion

    //        #region SQL Queries

    //        #region Union Table

    //        private string unionTable = string.Empty;

    //        private string queryUnionTable = @"
    //        select
    //		    *
    //	    from
    //		    [{0}] with (nolock)
    //    ";

    //        #endregion

    //        #region Get Attribute Schema

    //        private string queryGetAttributes = @"
    //        select distinct
    //	        [Key],
    //	        [Name],
    //	        [DataType]
    //        from
    //	        [fim].[AttributeInternal] as [ai] with (nolock)
    //        where
    //	        [ai].[Name] in ({0})
    //    ";

    //        #endregion

    //        #region Get searched Objects

    //        private string queryGetSearchedType = @"
    //        [type{1}] ([TypeKey])
    //        as
    //        (
    //			select
    //				[t].[Key]
    //			from
    //				[fim].[ObjectTypeInternal] as [t] with (nolock)
    //			where
    //				[t].Name = '{0}'
    //        ),
    //    ";

    //        private string queryGetSearchedObjects = @"
    //        with [actor{5}] ([ObjectKey])
    //        as
    //        (
    //	        select
    //		        [o].[ObjectKey]
    //	        from
    //		        [fim].[Objects] as [o] with (nolock)
    //	        where
    //		        [o].[ObjectID] = '{0}'
    //        ),
    //        {3}
    //        [rs{5}] ([ObjectKey], [ObjectID])	-- searched objects
    //        as
    //        (
    //	        select
    //		        [o].[ObjectKey],
    //		        [o].[ObjectID]
    //	        from
    //		        [fim].[Objects] as [o] with (nolock)
    //	        {1}
    //	        where
    //		        [o].[ObjectKey] is not null {4}
    //		        and
    //		        ({2})
    //		        and
    //		        (
    //			        [o].[ObjectKey] IN	-- Policy constraints on search object start
    //			        (
    //				        SELECT [smr].[ValueReference]
    //				        FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
    //				        WHERE
    //						        [smr].[ObjectTypeKey] = 29 /*Set*/
    //					        AND	[smr].[AttributeKey] = 40 /*ComputedMember*/
    //					        AND	[smr].[ObjectKey] IN
    //					        (
    //						        SELECT [mpr].[ResourceCurrentSet]
    //						        FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
    //						        WHERE
    //								        [mpr].[PrincipalSet] IN
    //								        (
    //									        SELECT [smP].[ObjectKey]
    //									        FROM [fim].[ObjectValueReference] AS [smP] with (nolock)
    //									        WHERE
    //											        [smP].[ObjectTypeKey] = 29 /*Set*/
    //										        AND	[smP].[AttributeKey] = 40 /*ComputedMember*/
    //										        AND	[smP].[ValueReference] = (select [ObjectKey] from [actor{5}] with (nolock)) /*The object key of the current principal*/
    //								        )
    //								        AND
    //								        (
    //									        [mpr].[ActionParameterAll] = 1
    //									        OR
    //									        [mpr].[ObjectKey] IN
    //									        (
    //										        SELECT [mpra].[ObjectKey]
    //										        FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
    //										        WHERE
    //												        [mpra].[ObjectKey] = [mpr].[ObjectKey]
    //											        AND	[mpra].[ActionParameterKey] = 66 /*DisplayName*/
    //									        )
    //								        )
    //					        )
    //			        )
    //			        OR
    //			        (
    //				        [o].[ObjectKey] IN
    //				        (
    //					        SELECT [smR].[ValueReference]
    //					        FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
    //					        WHERE
    //							        [smR].[ObjectTypeKey] = 29 /*Set*/
    //						        AND	[smR].[AttributeKey] = 40 /*ComputedMember*/
    //						        AND [smR].[ObjectKey] IN
    //						        (
    //							        SELECT [mpr].[ResourceCurrentSet]
    //							        FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
    //							        WHERE
    //									        [mpr].[PrincipalRelativeToResource]	IN
    //									        (
    //										        SELECT [ovrR].[AttributeKey]
    //										        FROM [fim].[ObjectValueReference] AS [ovrR] with (nolock)
    //										        WHERE
    //												        [ovrR].[ObjectKey] = [o].[ObjectKey]
    //											        AND	[ovrR].[ObjectTypeKey] = [o].[ObjectTypeKey]
    //											        AND	[ovrR].[ValueReference] = (select [ObjectKey] from [actor{5}] with (nolock)) /*The object key of the current principal*/
    //									        )
    //									        AND
    //									        (
    //										        [mpr].[ActionParameterAll] = 1
    //										        OR
    //										        [mpr].[ObjectKey] IN
    //										        (
    //											        SELECT [mpra].[ObjectKey]
    //											        FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
    //											        WHERE
    //													        [mpra].[ObjectKey] = [mpr].[ObjectKey]
    //												        AND	[mpra].[ActionParameterKey] = 66 /*DisplayName*/
    //										        )
    //									        )
    //						        )
    //				        )
    //			        )
    //		        )	-- Policy constraints on search object start
    //        )
    //    ";

    //        #endregion

    //        #region Get referenced Objects

    //        private string queryGetReferencedObjects = @"
    //        ,
    //        [rrs{1}] ([ObjectKey], [ObjectID])	-- referenced objects
    //        as
    //        (
    //        select distinct
    //	        [o].[ObjectKey],
    //	        [o].[ObjectID]
    //        from
    //	        [fim].[ObjectValueReference] as [r] with (nolock)
    //        right join
    //	        [fim].[Objects] as [o] with (nolock)
    //        on
    //	        [o].[ObjectKey] = [r].[ValueReference]
    //        where
    //	        [r].AttributeKey = {0}
    //	        and
    //	        [r].[ObjectKey] in (select [rs{1}].[ObjectKey] from [rs{1}] with (nolock))
    //	        and
    //	        (
    //		        [o].[ObjectKey] IN	-- Policy constraints on referenced object start
    //		        (
    //			        SELECT [smr].[ValueReference]
    //			        FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
    //			        WHERE
    //					        [smr].[ObjectTypeKey] = 29 /*Set*/
    //				        AND	[smr].[AttributeKey] = 40 /*ComputedMember*/
    //				        AND	[smr].[ObjectKey] IN
    //				        (
    //					        SELECT [mpr].[ResourceCurrentSet]
    //					        FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
    //					        WHERE
    //							        [mpr].[PrincipalSet] IN
    //							        (
    //								        SELECT [smP].[ObjectKey]
    //								        FROM [fim].[ObjectValueReference] AS [smP] with (nolock)
    //								        WHERE
    //										        [smP].[ObjectTypeKey] = 29 /*Set*/
    //									        AND	[smP].[AttributeKey] = 40 /*ComputedMember*/
    //									        AND	[smP].[ValueReference] = (select [ObjectKey] from [actor{1}] with (nolock)) /*The object key of the current principal*/
    //							        )
    //							        AND
    //							        (
    //								        [mpr].[ActionParameterAll] = 1
    //								        OR
    //								        [mpr].[ObjectKey] IN
    //								        (
    //									        SELECT [mpra].[ObjectKey]
    //									        FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
    //									        WHERE
    //											        [mpra].[ObjectKey] = [mpr].[ObjectKey]
    //										        AND	[mpra].[ActionParameterKey] = 66 /*DisplayName*/
    //								        )
    //							        )
    //				        )
    //		        )
    //		        OR
    //		        (
    //			        [o].[ObjectKey] IN
    //			        (
    //				        SELECT [smR].[ValueReference]
    //				        FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
    //				        WHERE
    //						        [smR].[ObjectTypeKey] = 29 /*Set*/
    //					        AND	[smR].[AttributeKey] = 40 /*ComputedMember*/
    //					        AND [smR].[ObjectKey] IN
    //					        (
    //						        SELECT [mpr].[ResourceCurrentSet]
    //						        FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
    //						        WHERE
    //								        [mpr].[PrincipalRelativeToResource]	IN
    //								        (
    //									        SELECT [ovrR].[AttributeKey]
    //									        FROM [fim].[ObjectValueReference] AS [ovrR] with (nolock)
    //									        WHERE
    //											        [ovrR].[ObjectKey] = [o].[ObjectKey]
    //										        AND	[ovrR].[ObjectTypeKey] = [o].[ObjectTypeKey]
    //										        AND	[ovrR].[ValueReference] = (select [ObjectKey] from [actor{1}] with (nolock)) /*The object key of the current principal*/
    //								        )
    //								        AND
    //								        (
    //									        [mpr].[ActionParameterAll] = 1
    //									        OR
    //									        [mpr].[ObjectKey] IN
    //									        (
    //										        SELECT [mpra].[ObjectKey]
    //										        FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
    //										        WHERE
    //												        [mpra].[ObjectKey] = [mpr].[ObjectKey]
    //											        AND	[mpra].[ActionParameterKey] = 66 /*DisplayName*/
    //									        )
    //								        )
    //					        )
    //			        )
    //		        )
    //	        )	-- Policy constraints on referenced object end
    //        )
    //    ";

    //        #endregion

    //        #region Get Attributes to load

    //        private string queryGetAttributesToLoad = @"
    //        ,
    //        [{0}] ([ObjectKey], [ObjectID], [AttributeKey], [ObjectTypeKey], [Value])	-- get value
    //        as
    //        (
    //	        select
    //		        [{5}].[ObjectKey],
    //		        [{5}].[ObjectID],
    //		        [{1}].[AttributeKey],
    //		        [{1}].[ObjectTypeKey],
    //		        cast([{1}].[{2}] as varchar(max))
    //	        from
    //		        [{5}] with (nolock)
    //	        left join
    //		        [fim].[{3}] as [{1}] with (nolock)
    //	        on
    //		        [{5}].[ObjectKey] = [{1}].[ObjectKey]
    //	        where
    //		        [{1}].[AttributeKey] in ({4})
    //		        and	-- Policy constraints on requested attributes start
    //		        (
    //			        [{1}].[ObjectKey] IN
    //			        (
    //				        SELECT [smr].[ValueReference]
    //				        FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
    //				        WHERE
    //						        [smr].[ObjectTypeKey] = 29 /*Set*/
    //					        AND	[smr].[AttributeKey] = 40 /*ComputedMember*/
    //					        AND	[smr].[ObjectKey] IN
    //					        (
    //						        SELECT [mpr].[ResourceCurrentSet]
    //						        FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
    //						        WHERE
    //								        [mpr].[PrincipalSet] IN
    //								        (
    //									        SELECT [smP].[ObjectKey]
    //									        FROM [fim].[ObjectValueReference] AS [smP] with (nolock)
    //									        WHERE
    //											        [smP].[ObjectTypeKey] = 29 /*Set*/
    //										        AND	[smP].[AttributeKey] = 40 /*ComputedMember*/
    //										        AND	[smP].[ValueReference] = (select [ObjectKey] from [actor{6}] with (nolock)) /*The object key of the current principal*/
    //								        )
    //								        AND
    //								        (
    //									        [mpr].[ActionParameterAll] = 1
    //									        OR
    //									        [mpr].[ObjectKey] IN
    //									        (
    //										        SELECT [mpra].[ObjectKey]
    //										        FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
    //										        WHERE
    //												        [mpra].[ObjectKey] = [mpr].[ObjectKey]
    //											        AND	[mpra].[ActionParameterKey] = [{1}].[AttributeKey] 
    //									        )
    //								        )
    //					        )
    //			        )
    //			        OR
    //			        (
    //				        [{1}].[ObjectKey] IN
    //				        (
    //					        SELECT [smR].[ValueReference]
    //					        FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
    //					        WHERE
    //							        [smR].[ObjectTypeKey] = 29 /*Set*/
    //						        AND	[smR].[AttributeKey] = 40 /*ComputedMember*/
    //						        AND [smR].[ObjectKey] IN
    //						        (
    //							        SELECT [mpr].[ResourceCurrentSet]
    //							        FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
    //							        WHERE
    //									        [mpr].[PrincipalRelativeToResource]	IN
    //									        (
    //										        SELECT [ovrR].[AttributeKey]
    //										        FROM [fim].[ObjectValueReference] AS [ovrR] with (nolock)
    //										        WHERE
    //												        [ovrR].[ObjectKey] = [{1}].[ObjectKey]
    //											        AND	[ovrR].[ObjectTypeKey] = [{1}].[ObjectTypeKey]
    //											        AND	[ovrR].[ValueReference] = (select [ObjectKey] from [actor{6}] with (nolock)) /*The object key of the current principal*/
    //									        )
    //									        AND
    //									        (
    //										        [mpr].[ActionParameterAll] = 1
    //										        OR
    //										        [mpr].[ObjectKey] IN
    //										        (
    //											        SELECT [mpra].[ObjectKey]
    //											        FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
    //											        WHERE
    //													        [mpra].[ObjectKey] = [mpr].[ObjectKey]
    //												        AND	[mpra].[ActionParameterKey] = [{1}].[AttributeKey] 
    //										        )
    //									        )
    //						        )
    //				        )
    //			        )
    //		        )	-- Policy constraints on requested attributes end
    //        )
    //    ";

    //        #endregion

    //        #region Get Reference Attributes to load

    //        private string queryGetReferenceAttributesToLoad = @"
    //        ,
    //        [{0}] ([ObjectKey], [ObjectID], [AttributeKey], [ObjectTypeKey], [Value])	-- get value
    //        as
    //        (
    //	        select
    //		        [{5}].[ObjectKey],
    //		        [{5}].[ObjectID],
    //		        [{1}].[AttributeKey],
    //		        [{1}].[ObjectTypeKey],
    //		        cast([{6}].[{7}] as varchar(max))
    //	        from
    //		        [{5}] with (nolock)
    //	        left join
    //		        [fim].[{3}] as [{1}] with (nolock)
    //	        on
    //		        [{5}].[ObjectKey] = [{1}].[ObjectKey]
    //            left join
    //				[fim].[Objects] as [{6}] with (nolock)
    //			on
    //				[{1}].[{2}] = [{6}].[ObjectKey]
    //	        where
    //		        [{1}].[AttributeKey] in ({4})
    //		        and	-- Policy constraints on requested attributes start
    //		        (
    //			        [{1}].[ObjectKey] IN
    //			        (
    //				        SELECT [smr].[ValueReference]
    //				        FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
    //				        WHERE
    //						        [smr].[ObjectTypeKey] = 29 /*Set*/
    //					        AND	[smr].[AttributeKey] = 40 /*ComputedMember*/
    //					        AND	[smr].[ObjectKey] IN
    //					        (
    //						        SELECT [mpr].[ResourceCurrentSet]
    //						        FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
    //						        WHERE
    //								        [mpr].[PrincipalSet] IN
    //								        (
    //									        SELECT [smP].[ObjectKey]
    //									        FROM [fim].[ObjectValueReference] AS [smP] with (nolock)
    //									        WHERE
    //											        [smP].[ObjectTypeKey] = 29 /*Set*/
    //										        AND	[smP].[AttributeKey] = 40 /*ComputedMember*/
    //										        AND	[smP].[ValueReference] = (select [ObjectKey] from [actor{8}] with (nolock)) /*The object key of the current principal*/
    //								        )
    //								        AND
    //								        (
    //									        [mpr].[ActionParameterAll] = 1
    //									        OR
    //									        [mpr].[ObjectKey] IN
    //									        (
    //										        SELECT [mpra].[ObjectKey]
    //										        FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
    //										        WHERE
    //												        [mpra].[ObjectKey] = [mpr].[ObjectKey]
    //											        AND	[mpra].[ActionParameterKey] = [{1}].[AttributeKey] 
    //									        )
    //								        )
    //					        )
    //			        )
    //			        OR
    //			        (
    //				        [{1}].[ObjectKey] IN
    //				        (
    //					        SELECT [smR].[ValueReference]
    //					        FROM [fim].[ObjectValueReference] AS [smr] with (nolock)
    //					        WHERE
    //							        [smR].[ObjectTypeKey] = 29 /*Set*/
    //						        AND	[smR].[AttributeKey] = 40 /*ComputedMember*/
    //						        AND [smR].[ObjectKey] IN
    //						        (
    //							        SELECT [mpr].[ResourceCurrentSet]
    //							        FROM [fim].[ManagementPolicyRuleReadInternal] AS [mpr] with (nolock)
    //							        WHERE
    //									        [mpr].[PrincipalRelativeToResource]	IN
    //									        (
    //										        SELECT [ovrR].[AttributeKey]
    //										        FROM [fim].[ObjectValueReference] AS [ovrR] with (nolock)
    //										        WHERE
    //												        [ovrR].[ObjectKey] = [{1}].[ObjectKey]
    //											        AND	[ovrR].[ObjectTypeKey] = [{1}].[ObjectTypeKey]
    //											        AND	[ovrR].[ValueReference] = (select [ObjectKey] from [actor{8}] with (nolock)) /*The object key of the current principal*/
    //									        )
    //									        AND
    //									        (
    //										        [mpr].[ActionParameterAll] = 1
    //										        OR
    //										        [mpr].[ObjectKey] IN
    //										        (
    //											        SELECT [mpra].[ObjectKey]
    //											        FROM [fim].[ManagementPolicyRuleReadInternalAttribute] AS [mpra] with (nolock)
    //											        WHERE
    //													        [mpra].[ObjectKey] = [mpr].[ObjectKey]
    //												        AND	[mpra].[ActionParameterKey] = [{1}].[AttributeKey] 
    //										        )
    //									        )
    //						        )
    //				        )
    //			        )
    //		        )	-- Policy constraints on requested attributes end
    //        )
    //    ";

    //        #endregion

    //        #region Get Final Result

    //        private string queryGetFinalResult = @"
    //        select
    //	        [frs].[ObjectID],
    //	        [ObjectType].[Name] as ObjectType,
    //	        [binding].[AttributeName],
    //            [attribute].[DataType] as [AttributeType],
    //	        [frs].[Value] as [AttributeValue],
    //	        [ovString].[ValueString] as [DisplayName],
    //	        [attribute].[Multivalued]
    //        from
    //	        [fim].[BindingInternal] as [binding] with (nolock)
    //        right join
    //        (
    //	        {0}
    //        ) as [frs]
    //        on
    //	        [binding].[ObjectTypeKey] = [frs].[ObjectTypeKey]
    //	        and
    //	        [binding].[AttributeKey] = [frs].[AttributeKey]
    //        left join
    //	        [fim].[ObjectValueString] as [ovString] with (nolock)
    //        on
    //	        [ovString].[AttributeKey] = 66
    //	        and
    //	        [ovString].[ObjectKey] = [frs].[ObjectKey]
    //	        and
    //	        [ovString].[LocaleKey] = 127
    //        left join
    //	        [fim].[AttributeInternal] as [attribute] with (nolock)
    //        on
    //	        [attribute].[Key] = [frs].[AttributeKey]
    //        left join
    //	        [fim].[ObjectTypeInternal] as [objecttype] with (nolock)
    //        on
    //	        [objecttype].[Key] = [frs].[ObjectTypeKey]
    //    ";

    //        #endregion

    //        #endregion

    //        #region Help methods

    //        private void parseAttribute()
    //        {
    //            foreach (Match match in Regex.Matches(this.XPathQuery, patternAttributeName, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase))
    //            {
    //                string value = match.Value.Trim();

    //                if (value.StartsWith("[") || value.StartsWith("("))
    //                {
    //                    value = value.Substring(1);
    //                }

    //                if (value.EndsWith("=") || value.EndsWith(","))
    //                {
    //                    value = value.Substring(0, value.Length - 1);
    //                }

    //                if (this.AttributeList.FirstOrDefault(a => a.Equals(value, StringComparison.OrdinalIgnoreCase)) == null)
    //                {
    //                    this.AttributeList.Add(value);
    //                }
    //            }
    //        }

    //        private void parseFunction()
    //        {
    //            foreach (Match match in Regex.Matches(this.XPathQuery, patternFunction, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase))
    //            {
    //                string value = match.Value.Trim();

    //                if (this.FunctionList.FirstOrDefault(f => f.Equals(value, StringComparison.OrdinalIgnoreCase)) == null)
    //                {
    //                    this.FunctionList.Add(value);
    //                }
    //            }
    //        }

    //        private void parseReference()
    //        {
    //            foreach (Match match in Regex.Matches(this.XPathQuery, patternReference, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase))
    //            {
    //                string value = match.Value.Trim();

    //                if (value.StartsWith("/"))
    //                {
    //                    value = value.Substring(1);
    //                }

    //                if (this.ReferenceList.FirstOrDefault(r => r.Equals(value, StringComparison.OrdinalIgnoreCase)) == null)
    //                {
    //                    this.ReferenceList.Add(value);
    //                }
    //            }
    //        }

    //        private void getSearchCriteria()
    //        {
    //            int beginPos = this.XPathQuery.IndexOf("[");
    //            int endPos = this.XPathQuery.LastIndexOf("]");

    //            this.SearchCriteria = this.XPathQuery.Substring(beginPos + 1, endPos - beginPos - 1);

    //            foreach (string function in this.FunctionList)
    //            {
    //                string[] opts = function.Split(new string[] { "(", ",", ")" }, StringSplitOptions.RemoveEmptyEntries);

    //                if (opts.Length != 3)
    //                {
    //                    continue;
    //                }

    //                string con = string.Empty;

    //                string value = opts[2].Trim();
    //                if (value.StartsWith("'") && value.EndsWith("'"))
    //                {
    //                    value = value.Substring(1, value.Length - 2);
    //                }
    //                else
    //                {
    //                    continue;
    //                }

    //                switch (opts[0].Trim().ToLower())
    //                {
    //                    case "contains":
    //                        con = string.Format("({0} like '%{1}%')", opts[1].Trim(), value);
    //                        break;
    //                    case "starts-with":
    //                        con = string.Format("({0} like '{1}%')", opts[1].Trim(), value);
    //                        break;
    //                    case "ends-with":
    //                        con = string.Format("({0} like '%{1}')", opts[1].Trim(), value);
    //                        break;
    //                    default:
    //                        break;
    //                }

    //                if (!string.IsNullOrEmpty(con))
    //                {
    //                    this.SearchCriteria = this.SearchCriteria.Replace(function, con);
    //                }
    //            }
    //        }

    //        private void getSQLQuerySerachedObjects(int level)
    //        {
    //            string conString = string.Empty;
    //            string conBoolean = string.Empty;
    //            string conReference = string.Empty;
    //            string conInteger = string.Empty;

    //            int cntString = 1;
    //            int cntBoolean = 1;
    //            int cntReference = 1;
    //            int cntInteger = 1;

    //            foreach (AttributeSchema att in this.SchemaList)
    //            {
    //                switch (att.Type.ToLower())
    //                {
    //                    case "string":

    //                        conString = string.Format(
    //                            @"  {0}
    //                            full join
    //	                            [fim].[ObjectValueString] as [string{1}] with (nolock)
    //                            on
    //	                            [string{1}].[ObjectKey] = [o].[ObjectKey] and [string{1}].[AttributeKey] = {2}
    //                        ",
    //                            conString, cntString, att.Key);

    //                        this.SearchCriteria = this.SearchCriteria.Replace(att.Name, string.Format("[string{0}].[ValueString]", cntString));

    //                        cntString++;

    //                        break;
    //                    case "integer":

    //                        conInteger = string.Format(
    //                            @"  {0}
    //                            full join
    //	                            [fim].[ObjectValueInteger] as [integer{1}] with (nolock)
    //                            on
    //	                            [integer{1}].[ObjectKey] = [o].[ObjectKey] and [integer{1}].[AttributeKey] = {2}
    //                        ",
    //                            conInteger, cntInteger, att.Key);

    //                        this.SearchCriteria = this.SearchCriteria.Replace(att.Name, string.Format("[integer{0}].[ValueInteger]", cntInteger));

    //                        cntString++;

    //                        break;
    //                    case "reference":

    //                        conReference = string.Format(
    //                            @"  {0}
    //                            full join
    //                                [fim].[ObjectValueReference] as [reference{1}] with (nolock)
    //                            on
    //                                [reference{1}].[AttributeKey] = {2} and [o].[ObjectKey] = [reference{1}].[ObjectKey]
    //                            full join
    //	                            [fim].[ObjectValueIdentifier] as [identifier{1}] with (nolock)
    //                            on
    //	                            [identifier{1}].[ObjectKey] = [reference{1}].[ValueReference]
    //                        ",
    //                            conReference, cntReference, att.Key);


    //                        foreach (Match match in Regex.Matches(this.SearchCriteria, string.Format(this.patternReferenceEqualOpt, att.Name)))
    //                        {
    //                            this.SearchCriteria = this.SearchCriteria.Replace(match.ToString(), string.Format("{0} in #", att.Name));
    //                        }
    //                        this.SearchCriteria = this.SearchCriteria.Replace(att.Name, string.Format("[identifier{0}].[ValueIdentifier]", cntReference));

    //                        cntReference++;

    //                        break;
    //                    case "boolean":

    //                        conBoolean = string.Format(
    //                            @"  {0}
    //                            full join
    //	                            [fim].[ObjectValueBoolean] as [boolean{1}] with (nolock)
    //                            on
    //	                            [boolean{1}].[ObjectKey] = [o].[ObjectKey] and [boolean{1}].[AttributeKey] = {2}
    //                        ",
    //                            conBoolean, cntBoolean, att.Key);

    //                        this.SearchCriteria = this.SearchCriteria.Replace(att.Name, string.Format("[boolean{0}].[ValueBoolean]", cntBoolean));

    //                        cntBoolean++;

    //                        break;
    //                    default:
    //                        break;
    //                }
    //            }

    //            string joins = string.Format("{0} \n{1} \n{2} \n{3}", conString, conBoolean, conReference, conInteger);

    //            this.SQLQuery = string.Format(this.queryGetSearchedObjects,
    //                this.ActorID, joins, this.SearchCriteria,
    //                this.SearchObjectType.Equals("*") ? string.Empty : string.Format(this.queryGetSearchedType, this.SearchObjectType, level),
    //                this.SearchObjectType.Equals("*") ? string.Empty : string.Format("and [o].[ObjectTypeKey] = (select [TypeKey] from [type{0}] with (nolock))", level),
    //                level);
    //        }

    //        private void getSQLQueryReferencedObjects(int level)
    //        {
    //            if (this.HasReference)
    //            {
    //                AttributeSchema att = this.SchemaList.FirstOrDefault(s => s.Name.Equals(this.ReferenceList.Last()));

    //                if (att != null)
    //                {
    //                    this.SQLQuery = string.Format("{0}{1}", this.SQLQuery, string.Format(this.queryGetReferencedObjects, att.Key, level));
    //                }
    //            }
    //        }

    //        private void getSQLQueryAttributesToLoad(int level)
    //        {
    //            // String Attributes
    //            string attString = string.Empty;
    //            foreach (AttributeSchema att in this.SchemaAttributesToLoad.Where(s => s.Type.Equals("string", StringComparison.OrdinalIgnoreCase)))
    //            {
    //                attString = string.IsNullOrEmpty(attString) ? att.Key : string.Format("{0},{1}", attString, att.Key);
    //            }
    //            if (!string.IsNullOrEmpty(attString))
    //            {
    //                this.SQLQuery = string.Format("{0}{1}",
    //                    SQLQuery,
    //                    string.Format(this.queryGetAttributesToLoad,
    //                        "StringValue", "ovString", "ValueString", "ObjectValueString", attString,
    //                        this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
    //                        level));

    //                this.unionTable = string.IsNullOrEmpty(this.unionTable) ?
    //                    string.Format(this.queryUnionTable, "StringValue") :
    //                    string.Format("{0}union{1}", this.unionTable, string.Format(this.queryUnionTable, "StringValue"));
    //            }

    //            // Boolean Attributes
    //            string attBoolean = string.Empty;
    //            foreach (AttributeSchema att in this.SchemaAttributesToLoad.Where(s => s.Type.Equals("boolean", StringComparison.OrdinalIgnoreCase)))
    //            {
    //                attBoolean = string.IsNullOrEmpty(attBoolean) ? att.Key : string.Format("{0},{1}", attBoolean, att.Key);
    //            }
    //            if (!string.IsNullOrEmpty(attBoolean))
    //            {
    //                this.SQLQuery = string.Format("{0}{1}",
    //                    SQLQuery,
    //                    string.Format(this.queryGetAttributesToLoad,
    //                        "BooleanValue", "ovBoolean", "ValueBoolean", "ObjectValueBoolean", attBoolean,
    //                        this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
    //                        level));

    //                this.unionTable = string.IsNullOrEmpty(this.unionTable) ?
    //                    string.Format(this.queryUnionTable, "BooleanValue") :
    //                    string.Format("{0}union{1}", this.unionTable, string.Format(this.queryUnionTable, "BooleanValue"));
    //            }

    //            // Integer Attributes
    //            string attInteger = string.Empty;
    //            foreach (AttributeSchema att in this.SchemaAttributesToLoad.Where(s => s.Type.Equals("integer", StringComparison.OrdinalIgnoreCase)))
    //            {
    //                attInteger = string.IsNullOrEmpty(attInteger) ? att.Key : string.Format("{0},{1}", attInteger, att.Key);
    //            }
    //            if (!string.IsNullOrEmpty(attInteger))
    //            {
    //                this.SQLQuery = string.Format("{0}{1}",
    //                    SQLQuery,
    //                    string.Format(this.queryGetAttributesToLoad,
    //                        "IntegerValue", "ovInteger", "ValueInteger", "ObjectValueInteger", attInteger,
    //                        this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level),
    //                        level));

    //                this.unionTable = string.IsNullOrEmpty(this.unionTable) ?
    //                    string.Format(this.queryUnionTable, "IntegerValue") :
    //                    string.Format("{0}union{1}", this.unionTable, string.Format(this.queryUnionTable, "IntegerValue"));
    //            }

    //            // Reference Attributes
    //            string attReference = string.Empty;
    //            foreach (AttributeSchema att in this.SchemaAttributesToLoad.Where(s => s.Type.Equals("reference", StringComparison.OrdinalIgnoreCase)))
    //            {
    //                attReference = string.IsNullOrEmpty(attReference) ? att.Key : string.Format("{0},{1}", attReference, att.Key);
    //            }
    //            if (!string.IsNullOrEmpty(attReference))
    //            {
    //                this.SQLQuery = string.Format("{0}{1}",
    //                    SQLQuery,
    //                    string.Format(this.queryGetReferenceAttributesToLoad,
    //                        "IdentifierValue", "ovReference", "ValueReference", "ObjectValueReference",
    //                        attReference, this.HasReference ? string.Format("rrs{0}", level) : string.Format("rs{0}", level), "ovIdentity", "ObjectID", level));

    //                this.unionTable = string.IsNullOrEmpty(this.unionTable) ?
    //                    string.Format(this.queryUnionTable, "IdentifierValue") :
    //                    string.Format("{0}union{1}", this.unionTable, string.Format(this.queryUnionTable, "IdentifierValue"));
    //            }
    //        }

    //        private void getSQLQueryFinalResult()
    //        {
    //            this.SQLQuery = string.Format("{0}{1}", this.SQLQuery, string.Format(this.queryGetFinalResult, this.unionTable));
    //        }

    //        #endregion

    //        #region Public Properties

    //        public string XPathQuery = string.Empty;

    //        public SqlConnection SQLConnection = null;

    //        public string ActorID = string.Empty;

    //        public bool IsValid = false;

    //        public string SearchObjectType = string.Empty;

    //        public bool HasReference = false;

    //        public List<string> AttributeList = new List<string>();

    //        public List<string> AttributesToLoad = new List<string>();

    //        public List<string> ReferenceList = new List<string>();

    //        public List<string> FunctionList = new List<string>();

    //        public string SearchCriteria = string.Empty;

    //        public List<AttributeSchema> SchemaList = new List<AttributeSchema>();

    //        public List<AttributeSchema> SchemaAttributesToLoad = new List<AttributeSchema>();

    //        public string SQLQuery = string.Empty;

    //        #endregion

    //        #region Constructor

    //        public XPathToSQL(string xpathQuery, string[] attributesToLoad, string actorID, SqlConnection conn)
    //        {
    //            this.XPathQuery = Regex.Replace(xpathQuery, patternReplaceBooleanTrue, "=1", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
    //            this.XPathQuery = Regex.Replace(this.XPathQuery, patternReplaceBooleanFalse, "=0", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

    //            foreach (Match match in Regex.Matches(this.XPathQuery, patternNotExpression, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase))
    //            {
    //                int posBegin = match.Value.IndexOf("(");
    //                int posEnd = match.Value.IndexOf("=");

    //                string attr = match.Value.Substring(posBegin + 1, posEnd - posBegin - 1);

    //                string replacement = string.Format("(({0} is null) or {1})", attr, match.Value);

    //                this.XPathQuery = this.XPathQuery.Replace(match.Value, replacement);
    //            }

    //            if (Regex.IsMatch(this.XPathQuery, patternValidXPath, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase) &&
    //                !string.IsNullOrEmpty(actorID) && conn != null)
    //            {
    //                this.IsValid = true;

    //                parseAttribute();
    //                parseFunction();
    //                parseReference();

    //                this.SearchObjectType = this.ReferenceList[0];

    //                if (this.ReferenceList.Count == 2)
    //                {
    //                    this.HasReference = true;
    //                }

    //                getSearchCriteria();

    //                this.AttributesToLoad = attributesToLoad.ToList();

    //                this.ActorID = actorID;

    //                this.SQLConnection = conn;
    //            }
    //        }

    //        #endregion

    //        #region Public Method

    //        public void GetAttributeSchema()
    //        {
    //            if (!this.IsValid)
    //            {
    //                return;
    //            }

    //            // get Search Attributes
    //            string attributeString = string.Empty;
    //            foreach (string att in this.AttributeList)
    //            {
    //                if (string.IsNullOrEmpty(attributeString))
    //                {
    //                    attributeString = string.Format("'{0}'", att);
    //                }
    //                else
    //                {
    //                    attributeString = string.Format("{0},'{1}'", attributeString, att);
    //                }
    //            }

    //            if (this.HasReference)
    //            {
    //                attributeString = string.Format("{0},'{1}'", attributeString, this.ReferenceList.Last());
    //            }

    //            SqlCommand cmd = new SqlCommand(string.Format(this.queryGetAttributes, attributeString), this.SQLConnection);

    //            SqlDataReader readerAttributeList = cmd.ExecuteReader();

    //            cmd.Dispose();

    //            while (readerAttributeList.Read())
    //            {
    //                this.SchemaList.Add(new AttributeSchema()
    //                {
    //                    Key = readerAttributeList["Key"].ToString(),
    //                    Name = readerAttributeList["Name"].ToString(),
    //                    Type = readerAttributeList["DataType"].ToString()
    //                });
    //            }

    //            readerAttributeList.Close();
    //            readerAttributeList.Dispose();


    //            // Get Attributes to load
    //            attributeString = string.Empty;
    //            foreach (string att in this.AttributesToLoad)
    //            {
    //                if (string.IsNullOrEmpty(attributeString))
    //                {
    //                    attributeString = string.Format("'{0}'", att);
    //                }
    //                else
    //                {
    //                    attributeString = string.Format("{0},'{1}'", attributeString, att);
    //                }
    //            }

    //            cmd = new SqlCommand(string.Format(this.queryGetAttributes, attributeString), this.SQLConnection);

    //            SqlDataReader readerAttributesToLoad = cmd.ExecuteReader();

    //            cmd.Dispose();

    //            while (readerAttributesToLoad.Read())
    //            {
    //                this.SchemaAttributesToLoad.Add(new AttributeSchema()
    //                {
    //                    Key = readerAttributesToLoad["Key"].ToString(),
    //                    Name = readerAttributesToLoad["Name"].ToString(),
    //                    Type = readerAttributesToLoad["DataType"].ToString()
    //                });
    //            }

    //            readerAttributesToLoad.Close();
    //            readerAttributesToLoad.Dispose();
    //        }

    //        public void GetSQLQuery(int level = 0, bool isEndResult = true)
    //        {
    //            getSQLQuerySerachedObjects(level);

    //            getSQLQueryReferencedObjects(level);

    //            if (isEndResult)
    //            {
    //                getSQLQueryAttributesToLoad(level);

    //                getSQLQueryFinalResult();
    //            }
    //            //else
    //            //{
    //            //    this.SQLQuery = string.Format("{0} select [ObjectKey] from [{1}{2}]", this.SQLQuery, this.HasReference ? "rrs" : "rs", level);
    //            //}
    //        }

    //        public string GetSubstiutionQuery(int level)
    //        {
    //            return string.Format("select [ObjectID] from [{0}{1}] with (nolock)", this.HasReference ? "rrs" : "rs", level);
    //        }

    //        #endregion

    //    }

    #endregion
}

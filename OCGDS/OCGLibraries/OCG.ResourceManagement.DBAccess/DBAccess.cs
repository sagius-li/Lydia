using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using OCG.ResourceManagement.ObjectModel;
using OCG.ResourceManagement.ObjectModel.ResourceTypes;

namespace OCG.ResourceManagement.DBAccess
{
    public class DBAccess
    {
        private class DBResource
        {
            public string ObjectID { get; set; }
            public string ObjectType { get; set; }
            public string DisplayName { get; set; }
            public string AttributeName { get; set; }
            public string AttributeType { get; set; }
            public string AttributeValue { get; set; }
            public bool IsMultivalue { get; set; }
        }

        public static RmAttribute GetAttribute(ref SqlConnection conn, string attributeName)
        {
            RmAttribute retVal = new RmAttribute();

            string query = @"
                select
                    Name,
                    DataType,
                    Multivalued
                from
                    fim.AttributeInternal with (nolock)
                where
                    Name='{0}'
            ";

            SqlCommand cmd = new SqlCommand(string.Format(query, attributeName), conn);

            SqlDataReader attributeReader = cmd.ExecuteReader();

            cmd.Dispose();

            while (attributeReader.Read())
            {
                retVal.Name = attributeReader[0].ToString();
                retVal.DataType = attributeReader[1].ToString();
                if (attributeReader[2] != null)
                {
                    retVal.Multivalued = (bool)attributeReader[2];
                }
                else
                {
                    retVal.Multivalued = false;
                }

                break;
            }

            attributeReader.Close();
            attributeReader.Dispose();

            return retVal;
        }

        public static List<RmAttribute> GetAttributes(ref SqlConnection conn, string objectType, int localeKey)
        {
            List<RmAttribute> retVal = new List<RmAttribute>();

            string query = @"
                with tblAttribute as
                (
	                select distinct
		                tblBinding.ObjectKey as [Key],
		                tblBinding.AttributeName as Name,
		                tblString.ValueString as DisplayName,
                        tblAttribute.DataType as DataType,
		                tblAttribute.Multivalued as IsMultivalue,
		                tblBinding.StringRegex as RegEx
	                from
		                fim.BindingInternal as tblBinding with (nolock)
                    inner join
						fim.AttributeInternal as tblAttribute with (nolock)
					on
						tblBinding.AttributeKey = tblAttribute.[Key]
	                inner join
		                fim.ObjectValueString as tblString with (nolock)
	                on
		                tblBinding.ObjectType='{0}' and 
		                tblString.AttributeKey=66 and 
		                tblString.LocaleKey={1} and 
		                tblBinding.ObjectKey=tblString.ObjectKey
                )
                select
	                tblAttribute.Name,
	                tblAttribute.DisplayName,
                    tblAttribute.DataType,
	                tblAttribute.IsMultivalue,
	                tblAttribute.RegEx,
	                tblString.ValueString as [Description]
                from
	                tblAttribute
                left join
	                fim.ObjectValueString as tblString with (nolock)
                on
	                tblString.ObjectKey=tblAttribute.[Key] and
	                tblString.AttributeKey=61 and
	                tblString.LocaleKey={1} and
	                tblString.ObjectTypeKey=5
            ";

            SqlCommand cmd = new SqlCommand(string.Format(query, objectType, localeKey), conn);

            SqlDataReader attributeReader = cmd.ExecuteReader();

            cmd.Dispose();

            while (attributeReader.Read())
            {
                RmAttribute attribute = new RmAttribute();
                if (attributeReader[0] != null && attributeReader[1] != null)
                {
                    attribute.Name = attributeReader[0].ToString();
                    attribute.DisplayName = attributeReader[1].ToString();

                    if (attributeReader[2] != null)
                    {
                        attribute.DataType = attributeReader[2].ToString();
                    }
                    else
                    {
                        attribute.Description = string.Empty;
                    }

                    if (attributeReader[3] != null)
                    {
                        attribute.Multivalued = (bool)attributeReader[3];
                    }
                    else
                    {
                        attribute.Multivalued = false;
                    }

                    if (attributeReader[5] != null)
                    {
                        attribute.Description = attributeReader[5].ToString();
                    }
                    else
                    {
                        attribute.Description = string.Empty;
                    }

                    if (attributeReader[4] != null)
                    {
                        attribute.RegEx = attributeReader[4].ToString();
                    }

                    retVal.Add(attribute);
                }
            }

            attributeReader.Close();
            attributeReader.Dispose();

            return retVal;
        }

        public static List<RmRight> GetRights(ref SqlConnection conn, Guid actor, Guid target, string dbName)
        {
            SqlCommand cmd = new SqlCommand(string.Format("[{0}].[fim].[GetInlineRights] @relativeViewPointGuid ,@targetGuid", dbName), conn);
            SqlParameter actorParam = cmd.Parameters.Add("@relativeViewPointGuid", System.Data.SqlDbType.UniqueIdentifier);
            SqlParameter targetParam = cmd.Parameters.Add("@targetGuid", System.Data.SqlDbType.UniqueIdentifier);
            actorParam.Value = actor;
            targetParam.Value = target;

            SqlDataReader rightReader = cmd.ExecuteReader();
            cmd.Dispose();

            string whereClause = string.Empty;
            List<RmRight> retVal = new List<RmRight>();

            while (rightReader.Read())
            {
                whereClause = string.Format("{0}{1}, ", whereClause, rightReader[0].ToString());

                RmRight entry = new RmRight();
                entry.AttributeID = int.Parse(rightReader[0].ToString());
                entry.ActionName = rightReader[1].ToString();
                retVal.Add(entry);
            }

            rightReader.Close();
            rightReader.Dispose();

            if (whereClause.Length > 0)
            {
                whereClause = whereClause.Trim();
                if (whereClause.EndsWith(","))
                {
                    whereClause = whereClause.Substring(0, whereClause.Length - 1);
                }
                string query = string.Format(string.Format("SELECT [Key],[Name],[Multivalued] FROM [{0}].[fim].[AttributeInternal] WHERE [Key] in ({1})", dbName, "{0}"), whereClause);
                cmd = new SqlCommand(query, conn);
                rightReader = cmd.ExecuteReader();
                cmd.Dispose();

                while (rightReader.Read())
                {
                    int id = int.Parse(rightReader[0].ToString());
                    string name = rightReader[1].ToString();
                    bool isMultivalue = rightReader[2].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase);

                    foreach (RmRight entry in retVal)
                    {
                        if (id == entry.AttributeID)
                        {
                            entry.AttributeName = name;
                            entry.IsMultivalue = isMultivalue;
                        }
                    }
                }

                rightReader.Close();
                rightReader.Dispose();
            }

            return retVal;

        }

        public static List<RmResource> GetResourceByQuery(ref SqlConnection conn, string requestorID, string xpath, string[] attributesToLoad, bool checkRights = true, int count = -1, int skip = 0)
        {
            
            XPathToSQLBuilder builder = new XPathToSQLBuilder(xpath, attributesToLoad, requestorID, conn);

            string sqlQuery = builder.BuildSQLQuery(checkRights, count, skip);

            var __resourceDict = new Dictionary<string, RmResource>();

            using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
            {

                using (SqlDataReader resourceReader = cmd.ExecuteReader())
                {

                    while (resourceReader.Read())
                    {
                        var __dbRes = new DBResource()
                        {
                            ObjectID = resourceReader[0].ToString(),
                            ObjectType = resourceReader[1].ToString(),
                            AttributeName = resourceReader[2].ToString(),
                            AttributeType = resourceReader[3].ToString(),
                            AttributeValue = resourceReader[4] == null ? string.Empty : resourceReader[4].ToString(),
                            DisplayName = resourceReader[5] == null ? string.Empty : resourceReader[5].ToString(),
                            IsMultivalue = Convert.ToBoolean(resourceReader[6])
                        };


                        RmResource __resource = null;

                        if (__resourceDict.ContainsKey(__dbRes.ObjectID))
                        {
                            __resource = __resourceDict[__dbRes.ObjectID];
                        }
                        else
                        {
                            __resource  = new RmResource()
                            {
                                ObjectID = new RmReference(__dbRes.ObjectID),
                                ObjectType = __dbRes.ObjectType,
                                DisplayName = __dbRes.DisplayName
                            };
                            __resourceDict.Add(__dbRes.ObjectID, __resource);
                        }

                        var __attName = new RmAttributeName(__dbRes.AttributeName);

                        if (!__resource.ContainsKey(__attName))
                        {
                            __resource.Attributes.Add(__attName, new RmAttributeValue() { Value = __dbRes.AttributeValue });
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(__dbRes.AttributeValue))
                            {
                                if (__resource[__attName].Values.Exists(v => v.ToString().Equals(__dbRes.AttributeValue,StringComparison.OrdinalIgnoreCase)))
                                {
                                    __resource[__attName].Values.Add(__dbRes.AttributeValue);
                                }
                            }

                        }

                    }
                }

            }
            return __resourceDict.Values.ToList();
        }
    }
}

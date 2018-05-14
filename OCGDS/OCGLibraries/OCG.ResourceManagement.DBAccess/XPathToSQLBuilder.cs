using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OCG.ResourceManagement.DBAccess
{
    #region Using Memory Table with Check-Rights-Switch

    public class XPathToSQLBuilder
    {
        #region Private Properties

        private string patternSubQuery = @"/((\w+)|\*)\[(?>\((?<DEPTH>)|\)(?<-DEPTH>)|(\[|\]|\\|/)(?<SC>)|.?)*(?(DEPTH)(?!))(?(SC)(?!))\](/\w+)?";

        private string substitutionQuery = string.Empty;

        #endregion

        #region Public Properties

        public string XPathQuery = string.Empty;

        public SqlConnection SQLConnection = null;

        public string ActorID = string.Empty;

        public List<string> AttributesToLoad = new List<string>();

        public string ResultSQLQuery = string.Empty;

        #endregion

        #region Help Functions

        private void recursiveBuildSQLQuery(string query, int level, bool checkRights = true, int count = -1, int skip = 0)
        {
            MatchCollection matches = Regex.Matches(query, this.patternSubQuery, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            // No Matches
            if (matches.Count == 0)
            {
                return;
            }

            if (matches.Count == 1)
            {
                foreach (Match match in matches)
                {
                    XPathToSQL parser = new XPathToSQL(match.ToString(), this.AttributesToLoad.ToArray(), this.ActorID, this.SQLConnection);
                    parser.GetAttributeSchema();

                    // End Result
                    if (match.ToString().Equals(query))
                    {
                        parser.GetSQLQuery(level, true, checkRights, count, skip);

                        if (string.IsNullOrEmpty(this.ResultSQLQuery))
                        {
                            this.ResultSQLQuery = parser.SQLQuery;
                        }
                        else
                        {
                            this.ResultSQLQuery = string.Format("{0}{1}",
                                this.ResultSQLQuery,
                                parser.SQLQuery.Replace(
                                    string.Format("#{0}#", level - 1),
                                    string.Format("({0})", this.substitutionQuery)));
                        }
                    }
                    // Sub Query
                    else
                    {
                        parser.GetSQLQuery(level, false, checkRights, count, skip);

                        if (string.IsNullOrEmpty(this.ResultSQLQuery))
                        {
                            this.ResultSQLQuery = parser.SQLQuery;
                        }
                        else
                        {
                            this.ResultSQLQuery = string.Format("{0}{1}",
                                this.ResultSQLQuery,
                                parser.SQLQuery.Replace(
                                    string.Format("#{0}#", level - 1),
                                    string.Format("({0})", this.substitutionQuery)));
                        }

                        this.substitutionQuery = parser.GetSubstiutionQuery(level);

                        recursiveBuildSQLQuery(query.Replace(match.ToString(), string.Format("#{0}#", level)), ++level, checkRights, count, skip);
                    }
                }
            }

            if (matches.Count > 1)
            {

            }
        }

        #endregion

        #region Public Methods

        public XPathToSQLBuilder(string xpathQuery, string[] attributesToLoad, string actorID, SqlConnection conn)
        {
            this.XPathQuery = xpathQuery;
            if (attributesToLoad != null)
            {
                this.AttributesToLoad = attributesToLoad.ToList();
            }
            else
            {
                this.AttributesToLoad = new List<string>() { "DisplayName" };
            }
            this.ActorID = actorID;
            this.SQLConnection = conn;
        }

        public string BuildSQLQuery(bool checkRights = true, int count = -1, int skip = 0)
        {
            recursiveBuildSQLQuery(this.XPathQuery, 0, checkRights, count, skip);

            return this.ResultSQLQuery;
        }

        #endregion
    }

    #endregion

    #region Using WITH Statement -- Obsoleted

    //public class XPathToSQLBuilder
    //{
    //    #region Private Properties

    //    private string patternSubQuery = @"/((\w+)|\*)\[(?>\((?<DEPTH>)|\)(?<-DEPTH>)|(\[|\]|\\|/)(?<SC>)|.?)*(?(DEPTH)(?!))(?(SC)(?!))\](/\w+)?";

    //    private string substitutionQuery = string.Empty;

    //    #endregion

    //    #region Public Properties

    //    public string XPathQuery = string.Empty;

    //    public SqlConnection SQLConnection = null;

    //    public string ActorID = string.Empty;

    //    public List<string> AttributesToLoad = new List<string>();

    //    public string ResultSQLQuery = string.Empty;

    //    #endregion

    //    #region Help Functions

    //    private void recursiveBuildSQLQuery(string query, int level)
    //    {
    //        MatchCollection matches = Regex.Matches(query, this.patternSubQuery, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

    //        // No Matches
    //        if (matches.Count == 0)
    //        {
    //            return;
    //        }

    //        if (matches.Count == 1)
    //        {
    //            foreach (Match match in matches)
    //            {
    //                XPathToSQL parser = new XPathToSQL(match.ToString(), this.AttributesToLoad.ToArray(), this.ActorID, this.SQLConnection);
    //                parser.GetAttributeSchema();

    //                // End Result
    //                if (match.ToString().Equals(query))
    //                {
    //                    parser.GetSQLQuery(level, true);

    //                    if (string.IsNullOrEmpty(this.ResultSQLQuery))
    //                    {
    //                        this.ResultSQLQuery = parser.SQLQuery;
    //                    }
    //                    else
    //                    {
    //                        this.ResultSQLQuery = string.Format("{0},{1}",
    //                            this.ResultSQLQuery,
    //                            parser.SQLQuery.TrimStart().Substring(5).Replace(
    //                                string.Format("#{0}#", level - 1),
    //                                string.Format("({0})", this.substitutionQuery)));
    //                    }
    //                }
    //                // Sub Query
    //                else
    //                {
    //                    parser.GetSQLQuery(level, false);

    //                    if (string.IsNullOrEmpty(this.ResultSQLQuery))
    //                    {
    //                        this.ResultSQLQuery = parser.SQLQuery;
    //                    }
    //                    else
    //                    {
    //                        this.ResultSQLQuery = string.Format("{0},{1}",
    //                            this.ResultSQLQuery,
    //                            parser.SQLQuery.TrimStart().Substring(5).Replace(
    //                                string.Format("#{0}#", level - 1),
    //                                string.Format("({0})", this.substitutionQuery)));
    //                    }

    //                    this.substitutionQuery = parser.GetSubstiutionQuery(level);

    //                    recursiveBuildSQLQuery(query.Replace(match.ToString(), string.Format("#{0}#", level)), ++level);
    //                }
    //            }
    //        }

    //        if (matches.Count > 1)
    //        {

    //        }
    //    }

    //    #endregion

    //    #region Public Methods

    //    public XPathToSQLBuilder(string xpathQuery, string[] attributesToLoad, string actorID, SqlConnection conn)
    //    {
    //        this.XPathQuery = xpathQuery;
    //        if (attributesToLoad != null)
    //        {
    //            this.AttributesToLoad = attributesToLoad.ToList();
    //        }
    //        else
    //        {
    //            this.AttributesToLoad = new List<string>() { "DisplayName" };
    //        }
    //        this.ActorID = actorID;
    //        this.SQLConnection = conn;
    //    }

    //    public string BuildSQLQuery()
    //    {
    //        recursiveBuildSQLQuery(this.XPathQuery, 0);

    //        return this.ResultSQLQuery;
    //    }

    //    #endregion
    //}

    #endregion
}

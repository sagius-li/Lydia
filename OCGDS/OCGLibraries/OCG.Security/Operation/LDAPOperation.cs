using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.DirectoryServices;

using OCG.Security.Authentication;

namespace OCG.Security.Operation
{
    public class LDAPOperation
    {
        public string GetStringValue(string attrName, AuthenticationOption option, SearchResultOption searchOption)
        {
            using (DirectorySearcher searcher = new DirectorySearcher())
            {
                try
                {
                    DirectoryEntry searchRoot = new DirectoryEntry();
                    searchRoot.Path = option.BuildLDAPPath();
                    searchRoot.Username = option.ServiceAccountName;
                    searchRoot.Password = option.ServiceAccountPwd;
                    searchRoot.AuthenticationType = option.GetAuthenticationType();

                    searcher.Filter = option.BuildSearchFilter();
                    searcher.PageSize = 1000;
                    searcher.PropertiesToLoad.Add(attrName);
                    searcher.SearchScope = SearchScope.Subtree;
                    searcher.SearchRoot = searchRoot;

                    SearchResultCollection searchResult = searcher.FindAll();

                    switch (searchOption)
                    {
                        case SearchResultOption.FindTheFirstOne:
                            if (searchResult != null && searchResult.Count > 0)
                            {
                                return searchResult[0].Properties[attrName][0].ToString();
                            }
                            break;
                        case SearchResultOption.FindTheOnlyOne:
                            if (searchResult != null && searchResult.Count == 1)
                            {
                                return searchResult[0].Properties[attrName][0].ToString();
                            }
                            break;
                        case SearchResultOption.FindAll:
                        default:
                            throw new NotImplementedException();
                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }

        public bool SetStringValue(string attrName, string attrValue, AuthenticationOption option, SearchResultOption searchOption)
        {
            using (DirectorySearcher searcher = new DirectorySearcher())
            {
                try
                {
                    DirectoryEntry searchRoot = new DirectoryEntry();
                    searchRoot.Path = option.BuildLDAPPath();
                    searchRoot.Username = option.ServiceAccountName;
                    searchRoot.Password = option.ServiceAccountPwd;
                    searchRoot.AuthenticationType = option.GetAuthenticationType();

                    searcher.Filter = option.BuildSearchFilter();
                    searcher.PageSize = 1000;
                    searcher.PropertiesToLoad.Add(attrName);
                    searcher.SearchScope = SearchScope.Subtree;
                    searcher.SearchRoot = searchRoot;

                    SearchResultCollection searchResult = searcher.FindAll();

                    switch (searchOption)
                    {
                        case SearchResultOption.FindTheFirstOne:
                            if (searchResult != null && searchResult.Count > 0)
                            {
                                DirectoryEntry de = searchResult[0].GetDirectoryEntry();
                                de.Properties[attrName].Value = attrValue;
                                de.CommitChanges();
                            }
                            break;
                        case SearchResultOption.FindTheOnlyOne:
                            if (searchResult != null && searchResult.Count == 1)
                            {
                                DirectoryEntry de = searchResult[0].GetDirectoryEntry();
                                de.Properties[attrName].Value = attrValue;
                                de.CommitChanges();
                            }
                            else
                            {
                                return false;
                            }
                            break;
                        case SearchResultOption.FindAll:
                        default:
                            throw new NotImplementedException();
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public enum SearchResultOption
    {
        FindAll = 0,
        FindTheFirstOne = 1,
        FindTheOnlyOne = 2
    }
}

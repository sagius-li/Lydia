using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.DirectoryServices;

using OCG.Security.Authentication;
using System.Management;

namespace OCG.Security.Operation
{
    public class ADOperation
    {
        public static string GetDomainName(string account)
        {
            if (account.Contains("\\") && account.Contains("@"))
            {
                return string.Empty;
            }

            if (account.Contains("\\"))
            {
                string[] accountInfo = account.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (accountInfo.Length == 2)
                {
                    return accountInfo[0];
                }
            }

            if (account.Contains("@"))
            {
                string[] accountInfo = account.Split("@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (accountInfo.Length == 2)
                {
                    return accountInfo[1];
                }
            }

            return string.Empty;
        }

        public static string GetAccountName(string account)
        {
            if (account.Contains("\\") && account.Contains("@"))
            {
                return string.Empty;
            }

            if (!account.Contains("\\") && !account.Contains("@"))
            {
                return account;
            }

            if (account.Contains("\\"))
            {
                string[] accountInfo = account.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (accountInfo.Length == 2)
                {
                    return accountInfo[1];
                }
            }

            if (account.Contains("@"))
            {
                string[] accountInfo = account.Split("@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (accountInfo.Length == 2)
                {
                    return accountInfo[0];
                }
            }

            return string.Empty;
        }

        public static string ResetPassword(string server, string domain, string samAccountName, string pwd, string maName)
        {
            string miisServer = string.Format(@"\\{0}\root\MicrosoftIdentityIntegrationServer", server);
            string miisQuery = string.Format(@"SELECT * FROM MIIS_CSObject WHERE Domain = '{0}' And account = '{1}'", domain, samAccountName);

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(miisServer, miisQuery);

                ManagementObjectCollection collection = searcher.Get();

                if (collection.Count == 0)
                {
                    throw new Exception("No Account was found");
                }

                foreach (ManagementObject user in collection)
                {
                    if (!user.Properties["MaName"].Value.ToString().Equals(maName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    ManagementBaseObject inParams = user.GetMethodParameters("SetPassword");
                    inParams["ForceChangeAtLogon"] = false;
                    inParams["UnlockAccount"] = true;
                    inParams["ValidatePassword"] = false;
                    inParams["NewPassword"] = pwd;

                    ManagementBaseObject outParams = user.InvokeMethod("SetPassword", inParams, null);

                    return outParams["ReturnValue"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            throw new Exception("Unknown error");
        }

        public void ResetPassword(string password, AuthenticationOption option)
        {
            ResetPassword(password, option, string.Empty);
        }

        public void ResetPassword(string password, AuthenticationOption option, string objectNotFoundExceptionText)
        {
            using (DirectorySearcher searcher = new DirectorySearcher())
            {
                DirectoryEntry searchRoot = new DirectoryEntry();
                searchRoot.Path = option.BuildLDAPPath();
                searchRoot.Username = option.ServiceAccountName;
                searchRoot.Password = option.ServiceAccountPwd;
                searchRoot.AuthenticationType = option.GetAuthenticationType();

                searcher.Filter = option.BuildSearchFilter();
                searcher.PageSize = 1000;
                searcher.SearchScope = SearchScope.Subtree;
                searcher.SearchRoot = searchRoot;

                SearchResultCollection searchResult = searcher.FindAll();

                if (searchResult != null && searchResult.Count == 1)
                {
                    searchResult[0].GetDirectoryEntry().Invoke("SetPassword", new object[] { password });
                }
                else
                {
                    throw new Exception(objectNotFoundExceptionText);
                }
            }
        }

        public void ResetPassword(string dn, string password, AuthenticationOption option)
        {
            DirectoryEntry user = new DirectoryEntry(string.Format("LDAP://{0}", dn), option.ServiceAccountName, option.ServiceAccountPwd, option.GetAuthenticationType());
            user.Invoke("SetPassword", password);
        }
    }
}

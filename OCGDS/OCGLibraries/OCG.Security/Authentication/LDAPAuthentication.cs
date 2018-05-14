using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using System.DirectoryServices;
using System.DirectoryServices.Protocols;

using OCG.Security.Operation;

namespace OCG.Security.Authentication
{
    public class LDAPAuthentication : IAuthentication
    {
        public bool Authenticate(string user, string password, string connectionString)
        {
            return Authenticate(user, password, new AuthenticationOption(connectionString));
        }

        public bool Authenticate(string password, AuthenticationOption option)
        {
            if (option == null)
            {
                return false;
            }

            try
            {
                LdapConnection con = new LdapConnection(option.ConnectionString);
                switch (option.Mode)
                {
                    case AuthenticationMode.None:
                    case AuthenticationMode.Anonymous:
                        con.AuthType = AuthType.Anonymous;
                        break;
                    case AuthenticationMode.Basic:
                        con.Credential = new NetworkCredential(option.ServiceAccountName, option.ServiceAccountPwd);
                        con.AuthType = AuthType.Basic;
                        break;
                    case AuthenticationMode.secure:
                        con.Credential = new NetworkCredential(
                            ADOperation.GetAccountName(option.ServiceAccountName), 
                            option.ServiceAccountPwd, 
                            ADOperation.GetDomainName(option.ServiceAccountName));
                        con.AuthType = AuthType.Ntlm;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                using (con)
                {
                    con.Bind();

                    string filter = option.BuildSearchFilter();
                    if (string.IsNullOrEmpty(filter))
                    {
                        return false;
                    }

                    System.DirectoryServices.Protocols.SearchRequest request = new System.DirectoryServices.Protocols.SearchRequest(
                        option.GetSearchRoot(),
                        filter,
                        System.DirectoryServices.Protocols.SearchScope.Subtree);

                    SearchResponse response = (SearchResponse)con.SendRequest(request);
                    if (response.Entries.Count != 1)
                    {
                        return false;
                    }
                    SearchResultEntry entry = response.Entries[0];
                    string dn = entry.DistinguishedName;

                    con.Credential = new NetworkCredential(dn, password);
                    con.AuthType = AuthType.Basic;
                    con.Bind();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Authenticate(string user, string password, AuthenticationOption option)
        {
            if (option.SearchAttributes.Count == 0)
            {
                Dictionary<string, string> searchAttributes = new Dictionary<string, string>();
                searchAttributes.Add("uid", user);
                searchAttributes.Add("userPrincipalName", user);
                searchAttributes.Add("cn", user);

                option.SearchOperator = AttributeSearchOperator.OR;
                option.SearchAttributes = searchAttributes;
            }

            return Authenticate(password, option);
        }
    }
}

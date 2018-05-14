using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.DirectoryServices;

namespace OCG.Security.Authentication
{
    public class AuthenticationOption
    {
        private Dictionary<string, string> searchAttributes = new Dictionary<string,string>();
        private string searchRoot = string.Empty;

        public string ConnectionString { get; set; }
        public string SearchRoot
        {
            set
            {
                searchRoot = value;
            }
        }
        public string ObjectClass { get; set; }

        public Dictionary<string, string> SearchAttributes
        {
            get { return searchAttributes; }
            set { searchAttributes = value; }
        }
        public AttributeSearchOperator SearchOperator { get; set; }

        public AuthenticationMode Mode { get; set; }
        public string ServiceAccountName { get; set; }
        public string ServiceAccountPwd { get; set; }

        public AuthenticationOption(string connectionString)
        {
            ConnectionString = connectionString;
            ObjectClass = @"person";
            SearchOperator = AttributeSearchOperator.AND;
            Mode = AuthenticationMode.None;
            ServiceAccountName = string.Empty;
            ServiceAccountPwd = string.Empty;
        }

        public string BuildSearchFilter()
        {
            string attrFilter = string.Empty;
            foreach (KeyValuePair<string, string> pair in SearchAttributes)
            {
                if (!string.IsNullOrEmpty(pair.Key) && !string.IsNullOrEmpty(pair.Value))
                {
                    attrFilter += string.Format("({0}={1})", pair.Key, pair.Value);
                }
            }

            if (string.IsNullOrEmpty(attrFilter))
            {
                return string.Empty;
            }

            switch (SearchOperator)
            {
                case AttributeSearchOperator.OR:
                    return string.Format("(&(objectClass={0})(|{1}))", ObjectClass, attrFilter);
                case AttributeSearchOperator.AND:
                default:
                    return string.Format("(&(objectClass={0})(&{1}))", ObjectClass, attrFilter);
            }
        }

        public string GetSearchRoot()
        {
            if (!string.IsNullOrEmpty(searchRoot))
            {
                return searchRoot;
            }

            if (string.IsNullOrEmpty(ConnectionString))
            {
                return string.Empty;
            }

            try
            {
                using (DirectoryEntry rootDSE = new DirectoryEntry("LDAP://" + ConnectionString + "/RootDSE"))
                {
                    if (rootDSE.Properties.Contains("defaultNamingContext") && rootDSE.Properties["defaultNamingContext"].Value != null)
                    {
                        return rootDSE.Properties["defaultNamingContext"].Value.ToString();
                    }
                    if (rootDSE.Properties.Contains("distinguishedName") && rootDSE.Properties["distinguishedName"].Value != null)
                    {
                        return rootDSE.Properties["distinguishedName"].Value.ToString();
                    }
                    if (rootDSE.Properties.Contains("namingContexts") && rootDSE.Properties["namingContexts"].Value != null)
                    {
                        Array namingContexts = rootDSE.Properties["namingContexts"].Value as Array;
                        if (namingContexts != null && namingContexts.Length > 2)
                        {
                            return namingContexts.GetValue(namingContexts.Length - 1).ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return string.Empty;
        }

        public string BuildLDAPPath()
        {
            string searchRoot = GetSearchRoot();
            return string.IsNullOrEmpty(searchRoot) ? string.Format("LDAP://{0}", ConnectionString) : string.Format("LDAP://{0}/{1}", ConnectionString, searchRoot);
        }

        public AuthenticationTypes GetAuthenticationType()
        {
            switch (Mode)
            {
                case AuthenticationMode.None:
                case AuthenticationMode.Anonymous:
                    return AuthenticationTypes.Anonymous;
                case AuthenticationMode.Basic:
                    return AuthenticationTypes.ServerBind;
                case AuthenticationMode.secure:
                    return AuthenticationTypes.Secure;
                default:
                    return AuthenticationTypes.None;
            }
        }
    }

    public enum AuthenticationMode
    {
        None = 0,
        Anonymous = 1,
        Basic = 2,         // LDAP
        secure =3          //AD
    }

    public enum AttributeSearchOperator
    {
        AND = 0,
        OR = 1
    }
}

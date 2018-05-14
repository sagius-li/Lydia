using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.Security.Authentication
{
    public class AuthenticationProvider
    {
        public static IAuthentication GetAuthenticationModule(AuthenticationProviderType type)
        {
            switch (type)
            {
                case AuthenticationProviderType.ActiveDirectory:
                    return new ADAuthentication();
                case AuthenticationProviderType.LDAP:
                    return new LDAPAuthentication();
                case AuthenticationProviderType.None:
                default:
                    throw new NotSupportedException("Authentication Provider Type is not supported");
            }
        }
    }

    public enum AuthenticationProviderType
    {
        None = 0,
        ActiveDirectory = 1,
        LDAP = 2
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.DirectoryServices;

namespace OCG.Security.Authentication
{
    public class ADAuthentication : IAuthentication
    {
        public bool Authenticate(string user, string password, string connectionString)
        {
            return Authenticate(user, password, new AuthenticationOption(connectionString));
        }

        public bool Authenticate(string password, AuthenticationOption option)
        {
            throw new NotImplementedException();
        }

        public bool Authenticate(string user, string password, AuthenticationOption option)
        {
            bool authenticated = false;

            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + option.ConnectionString, user, password);
                object nativeObject = entry.NativeObject;
                authenticated = true;
            }
            catch (Exception)
            {
            }

            return authenticated;
        }
    }
}

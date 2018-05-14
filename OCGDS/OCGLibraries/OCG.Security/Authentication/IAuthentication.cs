using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.Security.Authentication
{
    public interface IAuthentication
    {
        bool Authenticate(string user, string password, string connectionString);
        bool Authenticate(string password, AuthenticationOption option);
        bool Authenticate(string user, string password, AuthenticationOption option);
    }
}

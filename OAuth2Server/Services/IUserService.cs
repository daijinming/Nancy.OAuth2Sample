using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuth2Server
{
    public interface IUserService
    {
        Guid? ValidateUser(string username, string password);
        OAuth2UserIdentity FindUserByUsername(string username);
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuth2Server
{
    public class OAuthService:IOAuthService
    {

        public ApplicationClient FindClientById(string clientId)
        {   
            return new ApplicationClient { Name = "新浪网" };
        }

        public AuthCode CreateAuthCode(ApplicationClient client, OAuth2UserIdentity user)
        {   

            return new AuthCode { Code = "abc" };
        }

        public bool CheckAuthCode(string authCode)
        {   
            if (authCode.Length == 1)
                return false;
            else
                return true;
        }

        public IEnumerable<string> GetClientPermissions(string clientId)
        {   
            return new string[]{ "read","write" };
        }
    }
}
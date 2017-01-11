using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2Server
{
    public interface IOAuthService
    {   
        ApplicationClient FindClientById(string clientId);

        AuthCode CreateAuthCode(ApplicationClient client, OAuth2UserIdentity user);
        
        bool CheckAuthCode(string authCode);

        IEnumerable<string> GetClientPermissions(string clientId);

    }
}

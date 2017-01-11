using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Nancy.Security;

namespace OAuth2Server
{
    public class OAuth2UserIdentity : IUserIdentity
    {   
        public string UserName { get; set; }

        public IEnumerable<string> Claims { get; set; }


    }
}
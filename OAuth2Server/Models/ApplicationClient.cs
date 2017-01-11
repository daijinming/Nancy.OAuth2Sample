using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAuth2Server
{
    public class ApplicationClient
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string RedirectUri { get; set; }

    }
}
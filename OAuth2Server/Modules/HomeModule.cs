using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Extensions;
using Nancy.Authentication.Forms;
using Nancy.Security;

namespace OAuth2Server.Modules
{
    public class HomeModule:NancyModule
    {
        public HomeModule()
        {
            /*
            this.RequiresAuthentication();

            Get["/"] = x =>
            {   
                
                return this.Response.AsText("这是首页");
            };
            */            
        }
    }
}
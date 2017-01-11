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
    public class AccountModule:NancyModule
    {
        private IUserService _userService;

        public AccountModule(IUserService userService)
        {
            _userService = userService;

            Get["/Account/Login"] = x =>
            {
                return this.View["login.html"];
            };

            Post["/Account/Login"] = x =>
            {
                var userGuid = _userService.ValidateUser((string)this.Request.Form.Username, (string)this.Request.Form.Password);

                if (userGuid == null)
                {   
                    return Context.GetRedirect("/Account/Login?error=true&username=" + (string)this.Request.Form.Username);
                }

                DateTime? expiry = null;
                if (this.Request.Form.RememberMe.HasValue)
                {   
                    expiry = DateTime.Now.AddDays(7);
                }

                //this.Context.CurrentUser = new UserDatabase().GetUserFromIdentifier(userGuid.Value,this.Context);

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };
        }
    }
}
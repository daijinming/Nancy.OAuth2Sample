using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.OAuth2;
using Nancy.OAuth2.Services;
using Nancy.OAuth2.Modules;
using Nancy.OAuth2.Models;
using Nancy.OAuth2.Enums;
using Nancy.OAuth2.ModelBinders;
using Nancy.Authentication.Forms;

namespace OAuth2Server
{
    public class AuthorizationEndpointService : IAuthorizationEndpointService
    {   
        private readonly IOAuthService _oauthService;
        private readonly IUserService _userService;

        public AuthorizationEndpointService(
            IOAuthService oauthService,
            IUserService userService)
        {
            _oauthService = oauthService;
            _userService = userService;
        }

        public string GenerateAuthorizationCode(AuthorizationRequest request, NancyContext context)
        {   
            var client = _oauthService.FindClientById(request.ClientId);
            var user = _userService.FindUserByUsername(context.CurrentUser.UserName);
            
            var authCode = _oauthService.CreateAuthCode(client, user);

            return authCode.Code;
        }

        public OAuthValidationResult ValidateRequest(AuthorizationRequest request, NancyContext context)
        {
            var client = _oauthService.FindClientById(request.ClientId);

            if (client == null)
                return ErrorType.InvalidClient;

            // Perform validation of the request for the client e.g.
            // - Is the RedirectUri allowed?
            // - Does it support the authorization_code grant?

            return ErrorType.None;
        }

        

        public Tuple<string, object> GetCallbackView(AuthorizationRequest request, NancyContext context)
        {   

            if (!string.IsNullOrWhiteSpace(request.RedirectUri))
            {
                return new Tuple<string, object>("Callback", request.RedirectUri);
            }
            else
            {
                return new Tuple<string, object>("Error", request.RedirectUri);
            }
        }
        

        public Tuple<string, object> GetLoginView(AuthorizationRequest request, NancyContext context)
        {   

            return new Tuple<string, object>("Login", new object { });
        }

        public Tuple<Guid?, DateTime?> DoLogin(Request request, NancyContext context,NancyModule module)
        {   
            var userGuid = _userService.ValidateUser((string)request.Form.Username, (string)request.Form.Password);
            
            DateTime? expiry = null;
            if (request.Form.RememberMe.HasValue)
            {   
                expiry = DateTime.Now.AddDays(7);
            }
            
            return new Tuple<Guid?, DateTime?>(userGuid, expiry);
        }



        
    }
}
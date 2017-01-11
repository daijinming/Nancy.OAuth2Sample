using System;
using Nancy.ModelBinding;
using Nancy.OAuth2.Enums;
using Nancy.OAuth2.Models;
using Nancy.OAuth2.Services;
using Nancy.Security;
using Nancy.Authentication.Forms;

namespace Nancy.OAuth2.Modules
{
    public class AuthorizeModule : NancyModule
    {
        private readonly IAuthorizationEndpointService _service;
        private readonly IErrorResponseBuilder _errorResponseBuilder;

        public AuthorizeModule(
            IAuthorizationEndpointService service,
            IErrorResponseBuilder errorResponseBuilder)
            : base(OAuth.Configuration.GetFullPath(x => x.AuthorizationRequestRoute))
        {
            _service = service;
            _errorResponseBuilder = errorResponseBuilder;

            //this.RequiresAuthentication();
            
           
            
            Get["/", ctx => OAuth.IsEnabled] = _ => Index();

            Post["/", ctx => OAuth.IsEnabled] = _ => Login();
            
            //Post[OAuth.Configuration.AuthorizationAllowRoute, ctx => OAuth.IsEnabled] = _ => Allow();
            //Post[OAuth.Configuration.AuthorizationDenyRoute, ctx => OAuth.IsEnabled] = _ => Deny();
        }

        private dynamic Index()
        {   
            if (this.Context.CurrentUser == null)
            {
                return this.LoginShow();
            }

            var request = this.Bind<AuthorizationRequest>();

            var result = _service.ValidateRequest(request, Context);

            if (!result.IsValid)
            {   
                return Response.AsErrorResponse(BuildErrorResponse(result.ErrorType, request.State), request.RedirectUri);
            }

            Session[Context.CurrentUser.UserName] = request;

            var authorizationView = _service.GetCallbackView(request, Context);

            var authCode = _service.GenerateAuthorizationCode(request, Context);

            var response = new AuthorizationResponse
            {
                Code = authCode,
                State = request.State
            };

            var buri = new UriBuilder(request.RedirectUri) { Query = response.AsQueryString().TrimStart('?') };

            
            return View[authorizationView.Item1, new { Url = buri.ToString() }];
        }

        private dynamic Login()
        {
            var logView = _service.DoLogin(this.Request,this.Context,this);

            var guid = logView.Item1;

            if (guid.HasValue)
            {   
                return this.LoginAndRedirect(guid.Value, logView.Item2,this.Request.Url.ToString());
            }
            else 
                return Index();
        }

        private dynamic LoginShow()
        {   
            var request = this.Bind<AuthorizationRequest>();

            var loginView = _service.GetLoginView(request, Context);

            return View[loginView.Item1, loginView.Item2];
        }

        private dynamic Allow()
        {
            var request = Session[Context.CurrentUser.UserName] as AuthorizationRequest;
            
            if (request == null)
            {
                return HttpStatusCode.InternalServerError;
            }

            var authCode = _service.GenerateAuthorizationCode(request, Context);

            var response = new AuthorizationResponse
            {
                Code = authCode,
                State = request.State
            };

            var uri = new UriBuilder(request.RedirectUri) {Query = response.AsQueryString().TrimStart('?')};

            return Response.AsRedirect(uri.ToString());
        }

        private dynamic Deny()
        {
            var request = Session[Context.CurrentUser.UserName] as AuthorizationRequest;

            return request == null
                ? HttpStatusCode.InternalServerError
                : Response.AsErrorResponse(BuildErrorResponse(ErrorType.AccessDenied, request.State),
                    request.RedirectUri);
        }

        private ErrorResponse BuildErrorResponse(ErrorType errorType, string state = null)
        {
            return _errorResponseBuilder.Build(errorType, state);
        }
    }
}

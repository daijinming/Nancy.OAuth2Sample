using System;
using Nancy.OAuth2.Models;

namespace Nancy.OAuth2.Services
{
    public interface IAuthorizationEndpointService
    {   
        string GenerateAuthorizationCode(AuthorizationRequest request, NancyContext context);

        OAuthValidationResult ValidateRequest(AuthorizationRequest request, NancyContext context);

        Tuple<string, object> GetCallbackView(AuthorizationRequest request, NancyContext context);
        
        Tuple<string, object> GetLoginView(AuthorizationRequest request, NancyContext context);

        Tuple<Guid?, DateTime?> DoLogin(Request request, NancyContext context,NancyModule module);

    }
}

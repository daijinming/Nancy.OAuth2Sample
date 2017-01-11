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

namespace OAuth2Server
{
    public class TokenEndpointService : ITokenEndpointService
    {

        public IUserService _userService = null;
        public IOAuthService _oauthService = null;
        private readonly IAccessTokenStore accessTokenStore;

        public TokenEndpointService(IUserService userService,IOAuthService oauthService)
        {   
            _userService = userService;
            _oauthService = oauthService;
        }

        public OAuthValidationResult ValidateRequest(TokenRequest request, NancyContext context)
        {
            // Only allow certain grant types
            switch (request.GrantType)
            {
                case GrantTypes.Password:
                    // Check to see if the client credentials are valid
                    // (usually stored in Authorization header)
                    var guid = _userService.ValidateUser(request.Username, request.Password);
                    if (guid.HasValue)
                        return ErrorType.None;
                    else
                        return ErrorType.AccessDenied;
                    break;
                // OR return ErrorType.InvalidClient
                case GrantTypes.AuthorizationCode:
                    var code = request.Code;
                    var isValidate = _oauthService.CheckAuthCode(code);
                    if(!isValidate)
                        return ErrorType.AccessDenied;
                    break;
                default:
                    return ErrorType.InvalidGrant;
            }
            return ErrorType.None;
        }

        public TokenResponse CreateTokenResponse(TokenRequest request, NancyContext context)
        {   
            var token =
                 string.Concat("access-token-", Guid.NewGuid().ToString("D"));
            
            // 保存起来

            return new TokenResponse
            {   
                AccessToken = token
            };
        }


    }
}
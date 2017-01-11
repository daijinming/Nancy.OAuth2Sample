using System;
using System.Collections.Generic;
using Nancy.OAuth2.Enums;
using Nancy.OAuth2.Models;

namespace Nancy.OAuth2
{
    public interface IErrorResponseBuilder
    {
        ErrorResponse Build(ErrorType errorType, string state);
    }

    public class DefaultErrorResponseBuilder : IErrorResponseBuilder
    {
        private readonly IDictionary<ErrorType, Tuple<string, string>> _errorDescriptions;

        public DefaultErrorResponseBuilder()
        {
            _errorDescriptions = new Dictionary<ErrorType, Tuple<string, string>>
            {
                {
                    ErrorType.AccessDenied,
                    Tuple.Create("access_denied", "请求被拒绝.")
                },
                {
                    ErrorType.InvalidClient,
                    Tuple.Create("invalid_client", "客户端标识不正确.")
                },
                {
                    ErrorType.InvalidGrant,
                    Tuple.Create("invalid_grant", "不支持的授权类型.")
                },
                {
                    ErrorType.InvalidRequest,
                    Tuple.Create("invalid_request",
                        "请求参数有误.")
                },
                {
                    ErrorType.InvalidScope,
                    Tuple.Create("invalid_scope", "所请求的范围是无效的、 未知的、 或格式不正确.")
                },
                {
                    ErrorType.ServerError,
                    Tuple.Create("server_error",
                        "服务器遇到位置错误.")
                },
                {   
                    ErrorType.TemporarilyUnavailable,
                    Tuple.Create("temporarily_unavailable",
                        "服务器临时过载.")
                },
                {
                    ErrorType.UnauthorizedClient,
                    Tuple.Create("unauthorized_client",
                        "客户端未被授权.")
                },
                {
                    ErrorType.UnsupportedGrantType, 
                    Tuple.Create("invalid_grant", "不支持的授权模式.")
                },
                {
                    ErrorType.UnsupportedResponseType,
                    Tuple.Create("unsupported_response_type",
                        "不支持的返回类型.")
                }
            };
        }

        public ErrorResponse Build(ErrorType errorType, string state)
        {
            var descriptions = _errorDescriptions[errorType];

            return new ErrorResponse
            {
                Error = descriptions.Item1,
                ErrorDescription = descriptions.Item2,
                State = state
            };
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace FXV.CustomAuth
{
    public class CustomJwtBearerHandler : JwtBearerHandler
    {
        private OpenIdConnectConfiguration _configuration;

        public CustomJwtBearerHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, IDataProtectionProvider provider, ISystemClock clock)
            : base(options, logger, encoder, provider, clock)
        { }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = null;
            try
            {
                // Give application opportunity to find from a different location, adjust, or reject token
                var messageReceivedContext = new MessageReceivedContext(Context, Scheme, Options);

                // event can set the token
                await Events.MessageReceived(messageReceivedContext);
                if (messageReceivedContext.Result != null)
                {
                    return messageReceivedContext.Result;
                }

                // If application retrieved token from somewhere else, use that.
                token = messageReceivedContext.Token;

                if (string.IsNullOrEmpty(token))
                {
                    string authorization = Request.Headers["Authorization"];

                    // If no authorization header found, nothing to process further
                    if (string.IsNullOrEmpty(authorization))
                    {
                        return AuthenticateResult.NoResult();
                    }

                    if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        token = authorization.Substring("Bearer".Length).Trim();
                    }

                    // If no token found, no further work possible
                    if (string.IsNullOrEmpty(token))
                    {
                        return AuthenticateResult.NoResult();
                    }
                }

                if (_configuration == null && Options.ConfigurationManager != null)
                {
                    _configuration = await Options.ConfigurationManager.GetConfigurationAsync(Context.RequestAborted);
                }

                var validationParameters = Options.TokenValidationParameters.Clone();
                if (_configuration != null)
                {
                    var issuers = new[] { _configuration.Issuer };
                    validationParameters.ValidIssuers = validationParameters.ValidIssuers?.Concat(issuers) ?? issuers;

                    validationParameters.IssuerSigningKeys = validationParameters.IssuerSigningKeys?.Concat(_configuration.SigningKeys)
                        ?? _configuration.SigningKeys;
                }

                List<Exception> validationFailures = null;
                SecurityToken validatedToken;
                foreach (var validator in Options.SecurityTokenValidators)
                {
                    if (validator.CanReadToken(token))
                    {
                        ClaimsPrincipal principal;
                        try
                        {
                            principal = validator.ValidateToken(token, validationParameters, out validatedToken);

                            // checks if incoming Ip address is matched with the Ip address stored in jwt token
                            var ip = "Ip: " + Request.HttpContext.Connection.RemoteIpAddress;

                            if ((principal.Claims.ToList()[0].ToString() != null || principal.Claims.ToList()[0].ToString() != "") && !principal.Claims.ToList()[0].ToString().Equals(ip.ToString()))
                            {
                                validationFailures.Add(new Exception("Ip address is not matched"));
                            }
                        }
                        catch (Exception ex)
                        {
                            //Logger.TokenValidationFailed(ex);

                            if (validationFailures == null)
                            {
                                validationFailures = new List<Exception>(1);
                            }
                            validationFailures.Add(ex);
                            continue;
                        }

                        //Logger.TokenValidationSucceeded();

                        var tokenValidatedContext = new TokenValidatedContext(Context, Scheme, Options)
                        {
                            Principal = principal,
                            SecurityToken = validatedToken
                        };

                        await Events.TokenValidated(tokenValidatedContext);
                        if (tokenValidatedContext.Result != null)
                        {
                            return tokenValidatedContext.Result;
                        }

                        if (Options.SaveToken)
                        {
                            tokenValidatedContext.Properties.StoreTokens(new[]
                            {
                                new AuthenticationToken { Name = "access_token", Value = token }
                            });
                        }

                        tokenValidatedContext.Success();
                        return tokenValidatedContext.Result;
                    }
                }

                if (validationFailures != null)
                {
                    var authenticationFailedContext = new AuthenticationFailedContext(Context, Scheme, Options)
                    {
                        Exception = (validationFailures.Count == 1) ? validationFailures[0] : new AggregateException(validationFailures)
                    };

                    await Events.AuthenticationFailed(authenticationFailedContext);
                    if (authenticationFailedContext.Result != null)
                    {
                        return authenticationFailedContext.Result;
                    }

                    return AuthenticateResult.Fail(authenticationFailedContext.Exception);
                }

                return AuthenticateResult.Fail("No SecurityTokenValidator available for token: " + token ?? "[null]");
            }
            catch (Exception ex)
            {
                //Logger.ErrorProcessingMessage(ex);

                var authenticationFailedContext = new AuthenticationFailedContext(Context, Scheme, Options)
                {
                    Exception = ex
                };

                await Events.AuthenticationFailed(authenticationFailedContext);
                if (authenticationFailedContext.Result != null)
                {
                    return authenticationFailedContext.Result;
                }

                throw;
            }
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var authResult = await HandleAuthenticateOnceSafeAsync();

            var eventContext = new JwtBearerChallengeContext(Context, Scheme, Options, properties)
            {
                AuthenticateFailure = authResult?.Failure
            };

            // Avoid returning error=invalid_token if the error is not caused by an authentication failure (e.g missing token).
            if (Options.IncludeErrorDetails && eventContext.AuthenticateFailure != null)
            {
                eventContext.Error = "invalid_token";
                eventContext.ErrorDescription = CreateErrorDescription(eventContext.AuthenticateFailure);
            }

            await Events.Challenge(eventContext);

            if (eventContext.Handled)
            {
                return;
            }

            // replace the redirect URL with /Account/Index page if token does not pass the jwt token validation
            properties.RedirectUri = Request.Scheme + "://" + Request.Host + Request.PathBase + "/Account";

            //if (string.IsNullOrEmpty(eventContext.Error) &&
            //    string.IsNullOrEmpty(eventContext.ErrorDescription) &&
            //    string.IsNullOrEmpty(eventContext.ErrorUri))
            //{
            Response.Cookies.Delete("access_token");
            //    Response.Cookies.Append(HeaderNames.WWWAuthenticate, Options.Challenge);
            Response.Redirect(properties.RedirectUri);

            //}
            //else
            //{
            //    var builder = new StringBuilder(Options.Challenge);
            //    if (Options.Challenge.IndexOf(" ", StringComparison.Ordinal) > 0)
            //    {
            //        builder.Append(',');
            //    }
            //    if (!string.IsNullOrEmpty(eventContext.Error))
            //    {
            //        builder.Append(" error=\"");
            //        builder.Append(eventContext.Error);
            //        builder.Append("\"");
            //    }
            //    if (!string.IsNullOrEmpty(eventContext.ErrorDescription))
            //    {
            //        if (!string.IsNullOrEmpty(eventContext.Error))
            //        {
            //            builder.Append(",");
            //        }

            //        builder.Append(" error_description=\"");
            //        builder.Append(eventContext.ErrorDescription);
            //        builder.Append('\"');
            //    }
            //    if (!string.IsNullOrEmpty(eventContext.ErrorUri))
            //    {
            //        if (!string.IsNullOrEmpty(eventContext.Error) ||
            //            !string.IsNullOrEmpty(eventContext.ErrorDescription))
            //        {
            //            builder.Append(",");
            //        }

            //        builder.Append(" error_uri=\"");
            //        builder.Append(eventContext.ErrorUri);
            //        builder.Append('\"');
            //    }
            //    Response.Cookies.Delete("access_token");
            //    Response.Headers.Append(HeaderNames.WWWAuthenticate, builder.ToString());
            //    Response.Redirect(properties.RedirectUri);
            //}
        }

        private string CreateErrorDescription(Exception authenticateFailure)
        {
            return "";
        }
    }
}

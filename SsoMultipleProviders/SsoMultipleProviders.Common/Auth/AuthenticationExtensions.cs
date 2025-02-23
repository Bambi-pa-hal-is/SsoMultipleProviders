using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SsoMultipleProviders.Common.Auth
{
    public static class AuthenticationExtensions
    {
        public static AuthenticationBuilder AddOAuth(
            this IServiceCollection services, 
            Action<OpenIdConnectEvents, IAuthConstants> eventHandler,
            Action<OAuthEvents, IAuthConstants> oAuthEventHandler,
            List<IAuthConstants> authConstants)
        {
            var authBuilder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = null;
                options.DefaultSignInScheme = null;
                options.DefaultSignOutScheme = null;
            })
            .AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IAuthConstants.JwtSecret)),
                    ValidIssuer = IAuthConstants.Issuer,
                    ValidAudience = IAuthConstants.Audience,
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Read JWT token from cookie
                        var token = context.Request.Cookies["JWT_TOKEN"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            foreach (var authConstant in authConstants)
            {
                if(authConstant.UseOpenIdConnect)
                {
                    var openIdEvents = new OpenIdConnectEvents();
                    eventHandler(openIdEvents, authConstant);
                    authBuilder.AddOpenIdConnect(authConstant.AuthenticationScheme, options =>
                    {
                        options.ClientId = authConstant.ClientId;
                        options.Authority = authConstant.Authority;
                        options.ResponseType = OpenIdConnectResponseType.IdToken;
                        options.SaveTokens = true;
                        options.ClientSecret = authConstant.ClientSecret;
                        foreach (var scope in authConstant.Scopes)
                        {
                            options.Scope.Add(scope);
                        }
                        options.CallbackPath = authConstant.CallbackPath;
                        options.Events = openIdEvents;
                    });
                }
                else
                {
                    var oauthEvents = new OAuthEvents();
                    authBuilder.AddOAuth(authConstant.AuthenticationScheme, options =>
                    {
                        options.ClientId = authConstant.ClientId;
                        options.ClientSecret = authConstant.ClientSecret;
                        options.CallbackPath = new PathString(authConstant.CallbackPath);
                        options.SignInScheme = JwtBearerDefaults.AuthenticationScheme;
                        // For example, if using Discord-like endpoints:
                        options.AuthorizationEndpoint = "https://discord.com/api/oauth2/authorize";
                        options.TokenEndpoint = "https://discord.com/api/oauth2/token";
                        options.UserInformationEndpoint = "https://discord.com/api/users/@me";
                        options.SignInScheme = null;
                        // Add any requested scopes
                        foreach (var scope in authConstant.Scopes)
                        {
                            options.Scope.Add(scope);
                        }

                        // If you want token details stored, set this
                        options.SaveTokens = true;

                        oAuthEventHandler(oauthEvents, authConstant);
                        options.Events = oauthEvents;
                    });
                }
            }

            return authBuilder;
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using SsoMultipleProvider.Constants;
using SsoMultipleProviders.Client.Pages;
using SsoMultipleProviders.Common.Auth;
using SsoMultipleProviders.Common.Helpers;
using SsoMultipleProviders.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SsoMultipleProviders
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, ".env");
            ConfigurationHelpers.LoadEnvironmentFile(dotenv);

            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;
            services.AddHttpContextAccessor();

            // Add services to the container.
            services.AddRazorComponents().AddInteractiveWebAssemblyComponents();
            services.AddControllers();
            services.AddMediatR(x =>
            {
                x.RegisterServicesFromAssemblies(typeof(Program).Assembly);
            });
            services.AddAuthorizationCore(); // Required for Blazor Server auth
            services.AddCascadingAuthenticationState();
            services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            services.AddOAuth(
                (x, authConstants) =>
                {
                    x.OnTokenValidated = async (context) =>
                    {
                        var mediator = context.HttpContext.RequestServices.GetService<IMediator>();
                        var name = context?.Principal?.Claims.FirstOrDefault(x => x.Type == CustomClaimConstants.Name)?.Value ?? "No name";
                        var email = context?.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? "No Email";

                        //// Retrieve or create the user
                        //var user = await mediator!.Send(new LoginUser.Command()
                        //{
                        //    Name = name,
                        //    Email = email,
                        //});

                        // Generate a JWT token
                        var claims = new List<Claim>
                        {
                            new Claim(CustomClaimConstants.Name, name),
                            new Claim(ClaimTypes.Email, email)
                        };

                        var jwtHandler = new JwtSecurityTokenHandler();
                        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IAuthConstants.JwtSecret)); // Replace with your actual secret
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(claims),
                            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature),
                            Issuer = IAuthConstants.Issuer,
                            Audience = IAuthConstants.Audience
                        };

                        var token = jwtHandler!.CreateToken(tokenDescriptor);
                        var tokenString = jwtHandler.WriteToken(token);

                        // Set the JWT token as a cookie
                        context!.HttpContext.Response.Cookies.Append("JWT_TOKEN", tokenString, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true, // Use only HTTPS
                            SameSite = SameSiteMode.None,
                            Expires = tokenDescriptor.Expires
                        });

                        // Optionally remove the existing principal if you no longer want to use it
                        //context.Principal = null;
                        context!.Response!.Headers!.Add("Authorization", $"Bearer {tokenString}");
                        // Redirect and handle response
                        context.Response.Redirect(context.Properties.RedirectUri);
                        context.HandleResponse();
                    };
                    x.OnRedirectToIdentityProvider = context =>
                    {
                        context.ProtocolMessage.Prompt = OpenIdConnectPrompt.SelectAccount;
                        return Task.CompletedTask;
                    };
                },
                (x, authConstants) =>
                {
                    x.OnTicketReceived = async (context) =>
                    {
                        await Task.CompletedTask;
                        context.HandleResponse();
                    };
                    x.OnCreatingTicket = async (context) =>
                    {
                        var mediator = context.HttpContext.RequestServices.GetService<IMediator>();
                        var name = context?.Principal?.Claims.FirstOrDefault(x => x.Type == CustomClaimConstants.Name)?.Value ?? "No name";
                        var email = context?.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? "No Email";

                        //// Retrieve or create the user
                        //var user = await mediator!.Send(new LoginUser.Command()
                        //{
                        //    Name = name,
                        //    Email = email,
                        //});

                        // Generate a JWT token
                        var claims = new List<Claim>
                        {
                            new Claim(CustomClaimConstants.Name, name),
                            new Claim(ClaimTypes.Email, email)
                        };

                        var jwtHandler = new JwtSecurityTokenHandler();
                        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IAuthConstants.JwtSecret)); // Replace with your actual secret
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(claims),
                            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature),
                            Issuer = IAuthConstants.Issuer,
                            Audience = IAuthConstants.Audience
                        };

                        var token = jwtHandler!.CreateToken(tokenDescriptor);
                        var tokenString = jwtHandler.WriteToken(token);

                        // Set the JWT token as a cookie
                        context!.HttpContext.Response.Cookies.Append("JWT_TOKEN", tokenString, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true, // Use only HTTPS
                            SameSite = SameSiteMode.None,
                            Expires = tokenDescriptor.Expires
                        });

                        // Optionally remove the existing principal if you no longer want to use it
                        //context.Principal = null;
                        context!.Response!.Headers!.Add("Authorization", $"Bearer {tokenString}");
                        // Redirect and handle response
                        context.Response.Redirect("/");
                        //context.Result = HandleRequestResult.Handle();
                    };
                },
                new List<IAuthConstants> { new EntraAuthConstants(), new DiscordAuthConstants() }
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();
            app.UseMiddleware<JwtCookieToHeaderMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapRazorComponents<App>()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }
    }
}

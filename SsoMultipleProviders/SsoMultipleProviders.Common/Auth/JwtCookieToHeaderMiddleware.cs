using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Auth
{
    public class JwtCookieToHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtCookieToHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the JWT token exists in cookies
            if (context.Request.Cookies.TryGetValue("JWT_TOKEN", out var token))
            {
                // Set the Authorization header from the cookie if it does not contain bearer already.
                if (!context.Request.Headers.Authorization.ToString().Contains("bearer", StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Request.Headers.Append("Authorization", $"Bearer {token}");
                }
            }
            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}

namespace SsoMultipleProviders.Middlewares
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class JwtCookieToHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CookieName = "JWT_TOKEN"; // The name of your JWT cookie
        private const string AuthHeaderName = "Authorization";

        public JwtCookieToHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue(CookieName, out var token))
            {
                if (!string.IsNullOrEmpty(token) && !context.Request.Headers.ContainsKey(AuthHeaderName))
                {
                    context.Request.Headers[AuthHeaderName] = $"Bearer {token}";
                }
            }

            await _next(context);
        }
    }

}

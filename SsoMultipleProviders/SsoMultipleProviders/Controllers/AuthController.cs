using MediatR;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SsoMultipleProviders.Common.Controllers;
using SsoMultipleProviders.Common.Auth;

namespace SsoMultipleProviders.Controllers
{
    public class AuthController : ApiControllerBase
    {
        public AuthController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpGet("login/entra", Name = nameof(LoginEntra))]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public IActionResult LoginEntra([FromQuery] string redirectUri = "/")
        {
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUri },
                new EntraAuthConstants().AuthenticationScheme
            );
        }

        [AllowAnonymous]
        [HttpGet("login/discord", Name = nameof(LoginDiscord))]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public IActionResult LoginDiscord([FromQuery] string redirectUri = "/")
        {
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUri },
                new DiscordAuthConstants().AuthenticationScheme
            );
        }
    }
}

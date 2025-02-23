using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SsoMultipleProviders.Common.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiControllerBase : ControllerBase
    {
        public readonly IMediator _mediator;
        public ApiControllerBase(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult GetActionResult<T>(Response<T> response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response.Value);
                case HttpStatusCode.NoContent:
                    return NoContent();
                case HttpStatusCode.Redirect:
                    return Redirect(response.RedirectUrl ?? "");
                case HttpStatusCode.BadRequest:
                    return BadRequest(response.ErrorMessage ?? "");
                case HttpStatusCode.Forbidden:
                    return Forbid(JwtBearerDefaults.AuthenticationScheme);
                case HttpStatusCode.NotFound:
                    return NotFound(response.ErrorMessage);
            }
            return StatusCode(500, "Out of range, Update ControllerBase");
        }
    }
}

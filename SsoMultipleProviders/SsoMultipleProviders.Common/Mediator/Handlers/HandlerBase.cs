using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Mediator.Handlers
{
    public class HandlerBase<TResponse>
    {
        public Response<TResponse> Ok(TResponse value)
        {
            return Response<TResponse>.Ok(value);
        }

        public Response<TResponse> BadRequest(string errorMessage)
        {
            return Response<TResponse>.BadRequest(errorMessage);
        }

        public Response<TResponse> NotFound(string errorMessage)
        {
            return Response<TResponse>.NotFound(errorMessage);
        }

        public Response<TResponse> Redirect(TResponse result, string redirectUrl)
        {
            return Response<TResponse>.Redirect(result, redirectUrl);
        }

        public Response<TResponse> NoContent()
        {
            return Response<TResponse>.NoContent();
        }

        public Response<TResponse> Forbidden(string errorMessage)
        {
            return Response<TResponse>.Forbidden(errorMessage);
        }
    }
}

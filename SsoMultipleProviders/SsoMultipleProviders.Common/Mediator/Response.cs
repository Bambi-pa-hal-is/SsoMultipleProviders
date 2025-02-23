using System.Net;

namespace SsoMultipleProviders.Common.Mediator
{
    public struct Response<TResponse>
    {
        public HttpStatusCode StatusCode { get; set; }
        public TResponse? Value { get; set; }
        public string? ErrorMessage { get; set; }
        public string? RedirectUrl { get; set; }
        public Response(TResponse result)
        {
            Value = result;
            StatusCode = HttpStatusCode.OK;
            ErrorMessage = null;
            RedirectUrl = null;
        }

        public Response(HttpStatusCode httpStatusCode)
        {
            Value = default;
            StatusCode = httpStatusCode;
            ErrorMessage = null;
            RedirectUrl = null;
        }

        public Response(HttpStatusCode httpStatusCode, string errorMessage)
        {
            Value = default;
            StatusCode = httpStatusCode;
            ErrorMessage = null;
            RedirectUrl = null;
        }

        public bool HasError()
        {
            if ((int)StatusCode >= 200 && (int)StatusCode <= 299)
            {
                return false;
            }
            return true;
        }

        public static Response<TResponse> Ok(TResponse result)
        {
            return new Response<TResponse>(result);
        }

        public static Response<TResponse> BadRequest(string errorMessage)
        {
            return new Response<TResponse>(HttpStatusCode.BadRequest, errorMessage);
        }

        public static Response<TResponse> Redirect(TResponse result, string redirectUrl)
        {
            var response = new Response<TResponse>(HttpStatusCode.Redirect);
            response.RedirectUrl = redirectUrl;
            return response;
        }

        public static Response<TResponse> NotFound(string errorMessage)
        {
            return new Response<TResponse>(HttpStatusCode.NotFound, errorMessage);
        }

        public static Response<TResponse> Forbidden(string errorMessage)
        {
            return new Response<TResponse>(HttpStatusCode.Forbidden, errorMessage);
        }

        public static Response<TResponse> NoContent()
        {
            return new Response<TResponse>(HttpStatusCode.NoContent);
        }
    }
}

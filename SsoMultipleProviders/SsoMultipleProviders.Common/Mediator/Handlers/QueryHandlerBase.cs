using MediatR;
using SsoMultipleProviders.Common.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Mediator.Handlers
{
    public abstract class QueryHandlerBase<TRequest, TResponse> : HandlerBase<TResponse>, IRequestHandler<TRequest, Response<TResponse>>
        where TRequest : IRequest<Response<TResponse>>
    {
        public abstract Task<Response<TResponse>> Handle(TRequest query, CancellationToken cancellationToken);

    }
}

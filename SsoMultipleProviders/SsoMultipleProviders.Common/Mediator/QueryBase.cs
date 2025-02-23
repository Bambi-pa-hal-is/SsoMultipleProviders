using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Mediator
{
    public class QueryBase<TResponse> : IRequest<Response<TResponse>>
    {
    }
}

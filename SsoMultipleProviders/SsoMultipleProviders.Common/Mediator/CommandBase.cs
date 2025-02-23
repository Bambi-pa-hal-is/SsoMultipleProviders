using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Mediator
{
    public class CommandBase<TResponse> : IRequest<Response<TResponse>>
    {
    }
}

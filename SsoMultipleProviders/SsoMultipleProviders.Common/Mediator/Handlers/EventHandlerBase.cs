using MediatR;
using SsoMultipleProviders.Common.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Mediator.Handlers
{
    public abstract class EventHandlerBase<TEvent> : INotificationHandler<TEvent> where TEvent : EventBase
    {
        public abstract Task Handle(TEvent @event, CancellationToken cancellationToken);
    }
}

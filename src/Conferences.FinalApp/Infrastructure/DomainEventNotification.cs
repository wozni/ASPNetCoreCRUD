using MediatR;

namespace Initial.Infrastructure
{
    public class DomainEventNotification : INotification
    {
        public DomainEventNotification(object @event)
        {
            this.Event = @event;
        }
        
        public object Event { get; }
    }
}
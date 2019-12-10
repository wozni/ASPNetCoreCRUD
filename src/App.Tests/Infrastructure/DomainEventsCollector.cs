using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Initial.Infrastructure;
using MediatR;
using Xunit;

namespace App.Tests.Infractructure
{
    public class DomainEventsCollector : INotificationHandler<DomainEventNotification> 
    {
        private static readonly List<object> Events = new List<object>();

        public void ShouldContain<TEvent>(Predicate<TEvent> condition = null)
        {
            if (condition == null)
            {
                condition = evt => true;
            }
            Assert.Contains(Events.OfType<TEvent>(), e => condition(e));
        }
        
        public Task Handle(DomainEventNotification notification, CancellationToken cancellationToken)
        {
            Events.Add(notification.Event);
            return Task.CompletedTask;
        }
    }
}
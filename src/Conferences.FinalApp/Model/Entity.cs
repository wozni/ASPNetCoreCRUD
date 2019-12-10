using System.Collections.Generic;

namespace Initial.Models
{
    public class Entity
    {
        private readonly List<object> events = new List<object>();

        public IEnumerable<object> Events => events.AsReadOnly();

        protected void Publish(object @event) => events.Add(@event);
    }
}
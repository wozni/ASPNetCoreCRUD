using MediatR;

namespace Initial.Models
{
    public class ConferenceCreatedEvent 
    {
        public string Name { get; }

        public ConferenceCreatedEvent(Conference conference)
        {
            Name = conference.Name;
        }
    }
}
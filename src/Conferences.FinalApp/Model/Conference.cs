using System.Collections.Generic;
using System.Linq;

namespace Initial.Models
{
    public class Conference : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Session> Sessions { get; set; }

        public int SessionCount => Sessions.Count;

        public int AttendeeCount => Sessions.SelectMany(s => s.Attendees).Count();

        public IEnumerable<Attendee> GetAttendees() => Sessions.SelectMany(s => s.Attendees);

        public Conference(string name)
        {
            this.Name = name;
            Publish(new ConferenceCreatedEvent(this));
        }
        
        public void ChangeName(string newName)
        {
            this.Name = newName;
        }

        public Attendee GetAttendee(int id)
        {
            return GetAttendees().FirstOrDefault(a => a.Id == id);
        }
    }
}
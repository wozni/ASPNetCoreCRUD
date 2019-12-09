using System.Collections.Generic;
using System.Threading.Channels;

namespace Initial.Models
{
    public class ConferenceEditModel
    {
        public class AttendeeEditModel
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            
            public string EMail { get; set; }
        }

        public class SessionModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public List<AttendeeEditModel> Attendees { get; set; }
        public List<SessionModel> Sessions { get; set; }
    }
}
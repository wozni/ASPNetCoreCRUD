using System.Collections.Generic;

namespace Initial.Models
{
    public class SessionListModel
    {
        public class AttendeeModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; } 
        }

        public class SessionModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<AttendeeModel> Attendees { get; set; }    
        }
        
        public string ConferenceName { get; set; }
        public List<SessionModel> Sessions { get; set; }
        
    }
}
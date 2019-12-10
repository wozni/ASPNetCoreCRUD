using System.Collections.Generic;

namespace Initial.Models
{
    public class ConferenceRegisterAttendeeModel
    {
        public class SessionModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        
        public List<SessionModel> Sessions { get; set; }
        
        public string ConferenceName { get; set; }
        public int SessionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
    }
}
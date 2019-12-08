using System.Collections.Generic;

namespace Initial.Models
{
    public class ConferenceEditModel
    {
        public class AttendeeEditModel
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            
            public string LastName { get; set; }
        }
        
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public List<AttendeeEditModel> Attendees { get; set; }
        
    }
}
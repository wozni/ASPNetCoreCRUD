using System.Collections.Generic;

namespace Initial.Models
{
    public class Session
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public List<Attendee> Attendees { get; set; }
        
        public List<Speaker> Speakers { get; set; }
    }
}
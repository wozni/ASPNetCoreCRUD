namespace Initial.Models
{
    public class ConferenceRegisterAttendeeModel
    {
        public string ConferenceName { get; set; }
        public int SessionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }
    }
}
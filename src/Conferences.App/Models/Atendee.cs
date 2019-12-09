namespace Initial.Models
{
    public class Attendee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMail { get; set; }

        public Attendee(string firstName, string lastName, string eMail)
        {
            EMail = eMail;
            FirstName = firstName;
            LastName = lastName;
        }

        public void ChangeName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
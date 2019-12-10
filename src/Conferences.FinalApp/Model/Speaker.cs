namespace Initial.Models
{
    public class Speaker
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public Speaker(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
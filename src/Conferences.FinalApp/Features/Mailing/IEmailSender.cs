using Initial.Models;

namespace Initial.Services
{
    public interface IEmailSender
    {
        void NotifyAboutRegistration(Attendee attendee);
    }
}
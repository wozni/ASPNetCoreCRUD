using System;
using Initial.Models;

namespace Initial.Services
{
    class FakeEmailSender : IEmailSender
    {
        public void NotifyAboutRegistration(Attendee attendee)
        {
            Console.WriteLine($@"Email sent to {attendee.EMail}.");
        }
    }
}
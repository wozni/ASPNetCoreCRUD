using System;
using Initial.Models;
using Microsoft.Extensions.Logging;

namespace Initial.Services
{
    class FakeEmailSender : IEmailSender
    {
        private readonly ILogger<FakeEmailSender> logger;

        public FakeEmailSender(ILogger<FakeEmailSender> logger)
        {
            this.logger = logger;
        }
        
        public void NotifyAboutRegistration(Attendee attendee)
        {
            logger.LogInformation("Email send to @attendee.", attendee);
        }
    }
}
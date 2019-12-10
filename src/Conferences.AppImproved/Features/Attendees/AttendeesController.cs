using System.Linq;
using Initial.Models;
using Initial.Services;
using Microsoft.AspNetCore.Mvc;

namespace Initial.Features.Attendees
{
    public class AttendeesController : Controller
    {
        private readonly IConferenceRepository repository;
        private readonly IEmailSender emailSender;

        public AttendeesController(IConferenceRepository repository,
            IEmailSender emailSender)
        {
            this.repository = repository;
            this.emailSender = emailSender;
        }
        
        [HttpGet]
        public IActionResult RegisterAttendee(string conferenceName)
        {
            var conference = repository.GetByName(conferenceName);
            if (conference != null)
            {
                var model = new ConferenceRegisterAttendeeModel
                {
                    Sessions = conference.Sessions
                        .Select(s => new ConferenceRegisterAttendeeModel.SessionModel
                        {
                            Id = s.Id,
                            Name = s.Title
                        }).ToList(),
                    ConferenceName = conferenceName
                };
                return View("Index", model);
            }
            return RedirectToAction("Index", "Conferences");
        }
        
        
        [HttpPost]
        public IActionResult RegisterAttendee(ConferenceRegisterAttendeeModel form)
        {
            var conference = repository.GetByName(form.ConferenceName);
            var session = conference?.Sessions.FirstOrDefault(s => s.Id == form.SessionId);
            if (session != null)
            {
                var newAttendee = new Attendee(form.FirstName, form.LastName, form.EMail);
                session.Attendees.Add(newAttendee);
                repository.Update(conference);
                emailSender.NotifyAboutRegistration(newAttendee);
                return RedirectToAction("Edit", 
                    "Conferences",
                    new
                {
                    eventName = form.ConferenceName
                });
            }
            return View("Index");
        }
    }
}
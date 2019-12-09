using System.Linq;
using Initial.Models;
using Initial.Services;
using Microsoft.AspNetCore.Mvc;

namespace Initial.Controllers
{
    public class ConferencesController : Controller
    {
        private readonly IConferenceRepository repository;
        private readonly IEmailSender emailSender;

        public ConferencesController(IConferenceRepository repository,
            IEmailSender emailSender)
        {
            this.repository = repository;
            this.emailSender = emailSender;
        }
        
        public ActionResult Index(int? minSessions)
        {
            minSessions ??= 0;

            var list = (from conf in repository.GetAllConferences()
                where conf.SessionCount >= minSessions
                select new ConferenceListModel
                {
                    Id = conf.Id,
                    Name = conf.Name,
                    SessionCount = conf.SessionCount,
                    AttendeeCount = conf.AttendeeCount
                }).ToArray();
            return View(list);
        }
        
        public IActionResult Create()
        {
            return View("Create", new ConferenceCreateModel());
        }

        [HttpPost]
        public IActionResult Create(ConferenceCreateModel form)
        {
            if (ModelState.IsValid)
            {
                var conf = new Conference
                {
                    Name = form.Name
                };
                repository.Create(conf);
                return RedirectToAction("Index");
            }
            return View();
        }

        
        public ActionResult Edit(string eventName)
        {
            var conf = repository.GetByName(eventName);

            var model = new ConferenceEditModel
            {
                Id = conf.Id,
                Name = conf.Name,
                Sessions = conf.Sessions
                    .Select(s => new ConferenceEditModel.SessionModel
                    {
                        Id = s.Id,
                        Name = s.Title
                    }).ToList(),
                Attendees = conf.GetAttendees()
                    .Select(a => new ConferenceEditModel.AttendeeEditModel
                    {
                        Id = a.Id,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                        EMail = a.EMail
                    }).ToList()
            };

            return View(model);
        }

     
        [HttpPost]
        public ActionResult Edit(ConferenceEditModel form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            var conf = repository.GetById(form.Id);
            conf.ChangeName(form.Name);
            foreach (var attendeeEditModel in form.Attendees)
            {
                var attendee = conf.GetAttendee(attendeeEditModel.Id);
                attendee.ChangeName(attendeeEditModel.FirstName, attendeeEditModel.LastName);
            }
            repository.Update(conf);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RegisterAttendee(ConferenceRegisterAttendeeModel form)
        {
            var conference = repository.GetByName(form.ConferenceName);
            var session = conference.Sessions.FirstOrDefault(s => s.Id == form.SessionId);
            var newAttendee = new Attendee(form.FirstName, form.LastName, form.EMail);
            session.Attendees.Add(newAttendee);
            repository.Update(conference);
            emailSender.NotifyAboutRegistration(newAttendee);
            return RedirectToAction("Edit", new
            {
                eventName = form.ConferenceName
            });
        }
        
        [HttpGet]
        public IActionResult Delete(int id)
        {
            repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
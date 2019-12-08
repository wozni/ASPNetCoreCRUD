using System.Linq;
using Initial.Models;
using Initial.Services;
using Microsoft.AspNetCore.Mvc;

namespace Initial.Controllers
{
    public class ConferencesController : Controller
    {
        private readonly IConferenceRepository repository;

        public ConferencesController(IConferenceRepository repository)
        {
            this.repository = repository;
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
        
        public ActionResult Edit(string eventName)
        {
            var conf = repository.GetByName(eventName);

            var model = new ConferenceEditModel
            {
                Id = conf.Id,
                Name = conf.Name,
                Attendees = conf.GetAttendees()
                    .Select(a => new ConferenceEditModel.AttendeeEditModel
                    {
                        Id = a.Id,
                        FirstName = a.FirstName,
                        LastName = a.LastName,
                    }).ToList()
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View("Create");
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

        [HttpGet]
        public IActionResult Delete(int id)
        {
            repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
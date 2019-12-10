using System.Linq;
using System.Threading.Tasks;
using Initial.Features.Conferences;
using Initial.Models;
using Initial.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Initial.Features.Attendees
{
    public class AttendeesController : Controller
    {
        public class ConferenceRegisterAttendeeModel
        {
            public Get.ConferenceModel Conference { get; set; }
            public Register.Command Command { get; set; }
        }
        
        [HttpGet]
        public async Task<IActionResult> RegisterAttendee([FromServices] IMediator mediator, string conferenceName)
        {
            var response = await mediator.Send(new Get.Query()
            {
                Name = conferenceName
            });
            if (response.Conference != null)
            {
                var model = new ConferenceRegisterAttendeeModel
                {
                   Command = new Register.Command
                   {
                       ConferenceName = response.Conference.Name
                   },
                   Conference = response.Conference
                };
                return View("RegisterAttendee", model);
            }
            return RedirectToAction("Index", "Conferences");
        }
        
        [HttpPost]
        public async Task<IActionResult> RegisterAttendee([FromServices] IMediator mediator, ConferenceRegisterAttendeeModel form)
        {
            var response = await mediator.Send(form.Command);
            return RedirectToAction("Index", "Conferences");
        }
    }
}
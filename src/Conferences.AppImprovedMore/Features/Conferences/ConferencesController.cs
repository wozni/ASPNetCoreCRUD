using System.Linq;
using System.Threading.Tasks;
using Initial.Features.Conferences;
using Initial.Models;
using Initial.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Initial.Controllers
{
    public class ConferencesController : Controller
    {
        public async Task<ActionResult> Index([FromServices] IMediator mediator, [FromQuery]List.Query query)
        {
            var response = await mediator.Send(query);
            return View(response);
        }

        public IActionResult Create()
        {
            return View("Create", new Create.Command());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromServices] IMediator mediator, Create.Command form)
        {
            var response = await mediator.Send(form);
            if (response.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpGet("/Conferences/{eventName}")]
        public async Task<ActionResult> Edit([FromServices] IMediator mediator, string eventName)
        {
            var response = await mediator.Send(new Get.Query
            {
                Name = eventName
            });
            if (response.Conference != null)
            {
                var model = new Edit.Command
                {
                    Id = response.Conference.Id,
                    Name = response.Conference.Name,
                    Attendees = response.Conference.Attendees
                        .Select(a => new Edit.Command.AttendeeEditModel
                        {
                            Id = a.Id,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            EMail = a.Email
                        }).ToList()
                };
                return View(model);
            }

            return View("Index");
        }


        [HttpPost("/Conferences/{eventName}")]
        public async Task<ActionResult> Edit([FromServices] IMediator mediator, Edit.Command form)
        {
            var response = await mediator.Send(form);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromServices] IMediator mediator, int id)
        {
            await mediator.Send(new Delete.Command
            {
                ConferenceId = id
            });
            return RedirectToAction("Index");
        }
    }
}
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Initial.Features.Conferences;
using Initial.Models;
using Initial.Services;
using MediatR;

namespace Initial.Features.Attendees
{
    public static class Register
    {
        public class Command : IRequest<Response>
        {
            public string ConferenceName { get; set; }
            public int SessionId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string EMail { get; set; }
        }

        public class Response
        {
            public bool Succeeded { get; set; }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IConferenceRepository repository;
            private readonly IEmailSender emailSender;

            public Handler(IConferenceRepository repository, IEmailSender emailSender)
            {
                this.repository = repository;
                this.emailSender = emailSender;
            }

            public Task<Response> Handle(Command command, CancellationToken cancellationToken)
            {
                var conference = repository.GetByName(command.ConferenceName);
                var session = conference?.Sessions.FirstOrDefault(s => s.Id == command.SessionId);
                if (session != null)
                {
                    var newAttendee = new Attendee(command.FirstName, command.LastName, command.EMail);
                    session.Attendees.Add(newAttendee);
                    repository.Update(conference);
                    emailSender.NotifyAboutRegistration(newAttendee);
                }

                return Task.FromResult(new Response
                {
                    Succeeded = true
                });
            }
        }
    }
}
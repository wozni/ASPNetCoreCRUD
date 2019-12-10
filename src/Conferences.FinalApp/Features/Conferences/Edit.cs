using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Initial.Models;
using Initial.Services;
using MediatR;

namespace Initial.Features.Conferences
{
    public static class Edit
    {
        public class Command : IRequest<Response>
        {
            public class AttendeeEditModel
            {
                public int Id { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
            
                public string EMail { get; set; }
            }
        
            public int Id { get; set; }
        
            public string Name { get; set; }
        
            public List<AttendeeEditModel> Attendees { get; set; }
        }

        public class Response
        {
            public bool Succeeded { get; set; }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly ConferenceContext context;

            public Handler(ConferenceContext context)
            {
                this.context = context;
            }
            
            public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
            {
                var conf = context.GetById(command.Id);
                conf.ChangeName(command.Name);
                foreach (var attendeeEditModel in command.Attendees)
                {
                    var attendee = conf.GetAttendee(attendeeEditModel.Id);
                    attendee.ChangeName(attendeeEditModel.FirstName, attendeeEditModel.LastName);
                }

                return new Response
                {
                    Succeeded = true
                };
            }
        }
    }
}
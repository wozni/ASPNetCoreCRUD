using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Initial.Features.Attendees;
using Initial.Models;
using Initial.Services;
using MediatR;

namespace Initial.Features.Conferences
{
    public static class Create
    {
        public class Command : IRequest<Response>
        {
            public string Name { get; set; }
        }

        public class Response
        {
            public bool Succeeded { get; set; }    
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(ConferenceContext context)
            {
                RuleFor(command => command.Name)
                    .NotEmpty()
                    .WithMessage("Conference name cannot be empty.");
                RuleFor(command => command.Name)
                    .Must(context.ThereIsNoConferenceWithGivenName)
                    .WithMessage("There is already conference with given name.");
            }
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
                var response = new Response
                {
                    Succeeded = true
                };
                var conference = new Conference(command.Name);
                context.Conferences.Add(conference);
                return response;
            }
        }
    }
}
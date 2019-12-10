using System.Threading;
using System.Threading.Tasks;
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

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IConferenceRepository repository;

            public Handler(IConferenceRepository repository)
            {
                this.repository = repository;
            }
            
            public Task<Response> Handle(Command command, CancellationToken cancellationToken)
            {
                var response = new Response
                {
                    Succeeded = true
                };
                var conference = new Conference
                {
                    Name = command.Name
                };
                repository.Create(conference);
                return Task.FromResult(response);
            }
        }
    }
}
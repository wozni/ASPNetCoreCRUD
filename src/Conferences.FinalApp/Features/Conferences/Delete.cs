using System.Threading;
using System.Threading.Tasks;
using Initial.Models;
using Initial.Services;
using MediatR;

namespace Initial.Features.Conferences
{
    public static class Delete
    {
        public class Command : IRequest
        {
            public int ConferenceId { get; set; }
        }
        
        public class Handler : IRequestHandler<Command>
        {
            private readonly ConferenceContext context;

            public Handler(ConferenceContext context)
            {
                this.context = context;
            }
            
            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var conference = this.context.Conferences.Find(request.ConferenceId);
                if (conference != null)
                {
                    this.context.Conferences.Remove(conference);
                }
                return Unit.Task;
            }
        }
    }
}
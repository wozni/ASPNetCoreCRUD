using System.Threading;
using System.Threading.Tasks;
using Initial.Services;
using MediatR;

namespace Initial.Features.Conferences
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int ConferenceId { get; set; }
        }
        
        public class Handler : IRequestHandler<Command>
        {
            private readonly IConferenceRepository repository;

            public Handler(IConferenceRepository repository)
            {
                this.repository = repository;
            }
            
            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                repository.Delete(request.ConferenceId);
                return Unit.Task;
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Initial.Services;
using MediatR;

namespace Initial.Features.Conferences
{
    public static class List
    {
        public class Query : IRequest<Response>
        {
            public int? MinSessions { get; set; }
        }

        public class ConferenceModel
        {
            public int Id { get; set; }
        
            public string Name { get; set; }
        
            public int SessionCount { get; set; }
        
            public int AttendeeCount { get; set; }
        }
        
        public class Response
        {
            public List<ConferenceModel> Conferences { get; set; }
        }
        
        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IConferenceRepository repository;

            public Handler(IConferenceRepository repository)
            {
                this.repository = repository;
            }
            
            public Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                request.MinSessions ??= 0;
                var list = (from conf in repository.GetAllConferences()
                    where conf.SessionCount >= request.MinSessions
                    select new ConferenceModel()
                    {
                        Id = conf.Id,
                        Name = conf.Name,
                        SessionCount = conf.SessionCount,
                        AttendeeCount = conf.AttendeeCount
                    }).ToList();
                return Task.FromResult(new Response
                {
                    Conferences = list
                });
            }
        }
        
            
    }
}
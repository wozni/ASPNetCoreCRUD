using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Initial.Models;
using Initial.Services;
using MediatR;

namespace Initial.Features.Conferences
{
    public static class Get
    {
        public class Query : IRequest<Response>
        {
            public string Name { get; set; }
        }

        public class ConferenceModel
        {
            public class SessionModel
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }
            public class AttendeeModel
            {
                public int Id { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public string Email { get; set; }
            }
            
            public int Id { get; set; }
            public string Name { get; set; }
            
            public List<AttendeeModel> Attendees { get; set; }
            
            public List<SessionModel> Sessions { get; set; }
        }
        
        public class Response
        {
            public ConferenceModel Conference { get; set; }
        }
        
        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly ConferenceContext context;

            public Handler(ConferenceContext context)
            {
                this.context = context;
            }
            
            public Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var conf = context.GetByName(request.Name);
                return Task.FromResult(new Response
                {
                    Conference = new ConferenceModel
                    {
                        Id = conf.Id,
                        Name = conf.Name,
                        Sessions = conf.Sessions.Select(s => 
                            new ConferenceModel.SessionModel
                            {
                                Id = s.Id,
                                Name = s.Title
                            }).ToList(),
                        Attendees = conf.GetAttendees().Select(a =>
                            new ConferenceModel.AttendeeModel
                            {
                                Id = a.Id,
                                Email = a.EMail,
                                FirstName = a.FirstName,
                                LastName = a.LastName
                            }).ToList()
                    }
                });
            }
        }
    }
}
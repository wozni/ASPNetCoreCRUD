using System.Collections.Generic;
using System.Linq;
using Initial.Models;
using Microsoft.EntityFrameworkCore;

namespace Initial.Services
{
    class EntityFrameworkConferenceRepository : IConferenceRepository
    {
        private readonly ConferenceContext conferenceContext;

        public EntityFrameworkConferenceRepository(ConferenceContext conferenceContext)
        {
            this.conferenceContext = conferenceContext;
        }

        public IEnumerable<Conference> GetAllConferences()
        {
            return this.conferenceContext.Conferences
                .Include(c => c.Sessions)
                .ThenInclude(s => s.Attendees);
        }

        public Conference GetByName(string name)
        {
            return this.conferenceContext.Conferences
                .Include(c => c.Sessions)
                .ThenInclude(s => s.Attendees)
                .FirstOrDefault(c => c.Name == name);
        }

        public Conference GetById(int conferenceId)
        {
            return this.conferenceContext.Conferences
                .Include(c => c.Sessions)
                .ThenInclude(s => s.Attendees)
                .FirstOrDefault(c => c.Id == conferenceId);
        }

        public void Create(Conference conference)
        {
            conferenceContext.Conferences.Add(conference);
            conferenceContext.SaveChangesAsync();
        }

        public void Delete(int conferenceId)
        {
            var conference = this.conferenceContext.Conferences.Find(conferenceId);
            if (conference != null)
            {
                this.conferenceContext.Conferences.Remove(conference);
                this.conferenceContext.SaveChanges();
            }
        }

        public void Update(Conference conference)
        {
            this.conferenceContext.SaveChanges();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Initial.Models;
using Microsoft.EntityFrameworkCore;

namespace Initial.Services
{
    public interface IConferenceRepository
    {
        IEnumerable<Conference> GetAllConferences();
        Conference GetByName(string name);
        Conference GetById(int conferenceId);
        void Save(Conference conference);
    }

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

        public void Save(Conference conference)
        {
            this.conferenceContext.SaveChanges();
        }
    }
}
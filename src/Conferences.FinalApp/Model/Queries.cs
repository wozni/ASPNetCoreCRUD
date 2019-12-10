using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Initial.Models
{
    public static class Queries
    {
        public static Conference GetByName(this ConferenceContext context, string name) =>
            context.Conferences
                .Include(c => c.Sessions)
                .ThenInclude(s => s.Attendees)
                .FirstOrDefault(c => c.Name == name);
        
        public static Conference GetById(this ConferenceContext context, int conferenceId) =>
            context.Conferences
                .Include(c => c.Sessions)
                .ThenInclude(s => s.Attendees)
                .FirstOrDefault(c => c.Id == conferenceId);

        public static IQueryable<Conference> GetAllConferences(this ConferenceContext context) =>
            context.Conferences
                .Include(c => c.Sessions)
                .ThenInclude(s => s.Attendees);

        public static bool ThereIsNoConferenceWithGivenName(this ConferenceContext context, string name) =>
            context.Conferences.Count(c => c.Name == name) == 0;
    }
}
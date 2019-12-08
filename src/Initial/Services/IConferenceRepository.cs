using System.Collections.Generic;
using Initial.Models;

namespace Initial.Services
{
    public interface IConferenceRepository
    {
        IEnumerable<Conference> GetAllConferences();
        Conference GetByName(string name);
        Conference GetById(int conferenceId);

        void Create(Conference conference);

        void Delete(int conferenceId);
        void Update(Conference conference);
    }
}
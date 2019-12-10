using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Initial.Models
{
    public class ConferenceContext : DbContext
    {
        public DbSet<Conference> Conferences { get; set; }

        public ConferenceContext(DbContextOptions options) : base(options)
        {
        }

        public async Task SeedAsync()
        {
            var conferenceCount = await Conferences.CountAsync();
            if (conferenceCount > 0)
            {
                return;
            }

            var conference = new Conference
            {
                Name = "BitConf",
                Sessions = new List<Session>()
                {
                    new Session
                    {
                        Title = "REFACTOR AND DO IT SAFELY",
                        Speakers = new List<Speaker>
                        {
                            new Speaker("Jakub", "Pilimon")
                        },
                        Attendees = new List<Attendee>
                        {
                            new Attendee("Marcin", "Woźniak", "mwozniak@gmail.com"),
                            new Attendee("Adam", "Baszyński", "abaszynski@gmail.com"),
                        }
                            
                    }
                }
            };
            Conferences.Add(conference);
            await SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conference>(conference =>
            {
                conference.ToTable("Conferences");
                conference.HasMany(c => c.Sessions);
            });

            modelBuilder.Entity<Speaker>(speaker =>
            {
                speaker.ToTable("Speakers");
            });
            
            modelBuilder.Entity<Session>(session =>
            {
                session.ToTable("Sessions");
                session.HasMany(s => s.Attendees);
            });

            modelBuilder.Entity<Attendee>(attendee => { attendee.ToTable("Attendees"); });
        }
    }
}
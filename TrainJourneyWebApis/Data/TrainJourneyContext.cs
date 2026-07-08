using Microsoft.EntityFrameworkCore;
using TrainJourneyWebApis.Models;

namespace TrainJourneyWebApis.Data
{
    public class TrainJourneyContext : DbContext
    {
        public TrainJourneyContext(DbContextOptions<TrainJourneyContext> options) : base(options)
        {
        }

        public DbSet<JourneyStage> JourneyStages { get; set; }
        public DbSet<GeneratedStory> GeneratedStories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed the initial route data
            modelBuilder.Entity<JourneyStage>().HasData(
                new JourneyStage { Id = 1, Name = "Pretoria", Latitude = -25.7479, Longitude = 28.2293, SequenceOrder = 1 },
                new JourneyStage { Id = 2, Name = "Kimberley", Latitude = -28.7282, Longitude = 24.7623, SequenceOrder = 2 },
                new JourneyStage { Id = 3, Name = "Beaufort West", Latitude = -32.3445, Longitude = 22.5830, SequenceOrder = 3 },
                new JourneyStage { Id = 4, Name = "Worcester", Latitude = -33.6465, Longitude = 19.4459, SequenceOrder = 4 },
                new JourneyStage { Id = 5, Name = "Cape Town", Latitude = -33.9249, Longitude = 18.4241, SequenceOrder = 5 }
            );
        }
    }
}

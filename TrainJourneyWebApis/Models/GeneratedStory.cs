namespace TrainJourneyWebApis.Models
{
    public class GeneratedStory
    {
        public int Id { get; set; } // Primary Key
        public int JourneyStageId { get; set; } // Foreign Key to JourneyStage
        public string StoryText { get; set; } = string.Empty;
        public string StoryTitle { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public JourneyStage? JourneyStage { get; set; } // Navigation property to JourneyStage
    }
}

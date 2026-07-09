namespace TrainJourneyWebApis.DTOs
{
    public class EnrichedStoryMessage
    {
        public int StageId { get; set; }
        public string StoryTitle { get; set; } = string.Empty;
        public string StoryText { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;


    }
}

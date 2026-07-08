namespace TrainJourneyWebApis.Models
{
    public class JourneyStage
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int SequenceOrder { get; set; } // 1. Pretoria, 2. Johannesburg, 3. Cape Town... (just an example)
    }
}

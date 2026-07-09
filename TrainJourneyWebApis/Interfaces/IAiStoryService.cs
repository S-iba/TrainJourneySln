namespace TrainJourneyWebApis.Interfaces
{
    public interface IAiStoryService
    {
        Task<string> GenerateStoryAsync(string stationName);
    }
}

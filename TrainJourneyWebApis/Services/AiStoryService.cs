using System.Text;
using System.Text.Json;
using TrainJourneyWebApis.Interfaces;

namespace TrainJourneyWebApis.Services
{
    public class AiStoryService : IAiStoryService
    {
        private readonly HttpClient _httpClient;
        private const string LmStudioUrl = "http://localhost:1234/v1/chat/completions"; 

        public AiStoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GenerateStoryAsync(string stationName)
        {
            var requestBody = new
            {
                model = "qwen2.5-coder-3b-instruct", // TODO: Replace with a lightweight model suitable for this
                messages = new[]
                {
                    new { role = "system", content =
                    "You are a creative storyteller." }, //TODO: Needs more context about the station, the train journey, and the passengers to generate a story.
                    new { role = "user", content =
                    $"Write a story about {stationName}." }
                },
                temperature = 0.7
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync(LmStudioUrl, content);

                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();

                using var jsonDoc = JsonDocument.Parse(responseString);

                var storyText = jsonDoc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                return storyText ?? "No story generated.";

            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"Error generating story: {ex.Message}");
                return "Error generating story.";

            }

        }
    }
}

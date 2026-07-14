using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TrainJourneyWebApis.Data;
using TrainJourneyWebApis.DTOs;
using TrainJourneyWebApis.Interfaces;
using TrainJourneyWebApis.Models;

namespace TrainJourneyWebApis.Workers
{
    public class TelemetryConsumerWorker : BackgroundService
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IAiStoryService _aiService;
        private IConnection _connection = null!;
        private IChannel _channel = null!;

        public TelemetryConsumerWorker(IConnectionFactory connectionFactory, IServiceScopeFactory scopeFactory, IAiStoryService aiService)
        {
            _connectionFactory = connectionFactory;
            _scopeFactory = scopeFactory;
            _aiService = aiService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection = await _connectionFactory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageString = Encoding.UTF8.GetString(body);
                var telemetry = JsonSerializer.Deserialize<TelemetryMessage>(messageString);

                if (telemetry != null)
                {
                    await ProcessTelemetryAsync(telemetry);
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            };

            await _channel.BasicConsumeAsync(queue: "telemetry.processing.queue", autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        }

        private async Task ProcessTelemetryAsync(TelemetryMessage telemetry)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TrainJourneyContext>();

            var closestStage = await dbContext.JourneyStages
                .OrderBy(s => Math.Pow(s.Latitude - telemetry.Latitude, 2) + Math.Pow(s.Longitude - telemetry.Longitude, 2))
                .FirstOrDefaultAsync();

            if (closestStage == null)
            {
                return;
            }

            var storyExists = await dbContext.GeneratedStories.AnyAsync(s => s.JourneyStageId == closestStage.Id);
            if (storyExists) return;

            string storyText = await _aiService.GenerateStoryAsync(closestStage.Name);
            string mockImageUrl = $"https://example.com/images/{closestStage.Name.Replace(" ", "_")}.jpg";

            var newStory = new GeneratedStory
            {
                JourneyStageId = closestStage.Id,
                StoryTitle = storyText,
                StoryText = storyText,
                ImageUrl = mockImageUrl
            };

            dbContext.GeneratedStories.Add(newStory);
            await dbContext.SaveChangesAsync();

            await PublishEnrichedStory(new EnrichedStoryMessage
            {
                StageId = closestStage.Id,
                StoryTitle = storyText,
                StoryText = storyText,
                ImageUrl = mockImageUrl
            });
        }

        private async Task PublishEnrichedStory(EnrichedStoryMessage message)
        {
            if (_channel == null) return;

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await _channel.BasicPublishAsync(
                exchange: "journey.exchange",
                routingKey: "journey.stories.enriched",
                mandatory: false,
                basicProperties: (BasicProperties)null,
                body: body,
                cancellationToken: CancellationToken.None
            );
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            if (_channel is not null)
            {
                await _channel.CloseAsync(cancellationToken: stoppingToken);
            }

            if (_connection is not null)
            {
                await _connection.CloseAsync(cancellationToken: stoppingToken);
            }

            await base.StopAsync(stoppingToken);
        }

        public async override void Dispose()
        {
            await _channel?.CloseAsync();
            await _connection?.CloseAsync();
            base.Dispose();
        }                       
    }
}

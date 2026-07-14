using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TrainJourneyWebApis.Data;
using TrainJourneyWebApis.DTOs;

namespace TrainJourneyWebApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneyController : ControllerBase
    {
        private readonly TrainJourneyContext _dbContext;
        private readonly IConnectionFactory _connectionFactory;

        public JourneyController(TrainJourneyContext dbContext, IConnectionFactory connectionFactory)
        {
            _dbContext = dbContext;
            _connectionFactory = connectionFactory;
        }

        [HttpPost("telemetry")]
        public async Task<IActionResult> PostTelemetry([FromBody] TelemetryMessage telemetry)
        {
             var connection = await _connectionFactory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            var json = JsonSerializer.Serialize(telemetry);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync("journey.exchange", "journey.telemetry", false, (BasicProperties)null, body,CancellationToken.None);
            return Accepted();
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetCurrentStatusAsync()
        {
            var latestStory = await _dbContext.GeneratedStories
                .Include(s => s.JourneyStage)
                .OrderByDescending(s => s.GeneratedAt)
                .FirstOrDefaultAsync();

            if (latestStory?.JourneyStage == null)
            {
                return Ok(new { stageId = 0, stageName = "Awaiting Departure" });
            }


            return Ok(new
            {
                stageId = latestStory.JourneyStage.Id,
                stageName = latestStory.JourneyStage.Name,
                SequencePosition = latestStory.JourneyStage.SequenceOrder

            });
        }


    }
}

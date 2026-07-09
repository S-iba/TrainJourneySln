using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using TrainJourneyWebApis.Data;
using TrainJourneyWebApis.Hubs;
using TrainJourneyWebApis.Services;
using TrainJourneyWebApis.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add this under your existing builder.Services statements:
builder.Services.AddHttpClient<AiStoryService>();
builder.Services.AddHostedService<TelemetryConsumerWorker>();

builder.Services.AddDbContext<TrainJourneyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDb")));

builder.Services.AddSingleton<IConnectionFactory>(sp =>
{
    var factory = new ConnectionFactory();
    factory.Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMq")!);
    return factory;
});

builder.Services.AddSignalR();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<JourneyHub>("/journeyhub");

app.Run();

using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using TrainJourneyWebApis.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TrainJourneyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

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

app.Run();

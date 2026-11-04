using Messaging.RabbitMQ;
using SubscriberService.Persistence;
using SubscriberService.ReleaseToggles;
using SubscriberService.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISubscriberService, SubscriberService.Services.SubscriberService>();
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);
await builder.Services.AddFeatureHubAsync(builder.Configuration);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()));

builder.Services.AddSwaggerGen();    

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<SubscriberReleaseToggleMiddleware>();

app.MapControllers();

app.Run();

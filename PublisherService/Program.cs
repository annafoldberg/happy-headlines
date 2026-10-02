using Messaging.RabbitMQ;
using Monitoring;
using PublisherService.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPublisherService, PublisherService.Services.PublisherService>();
builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddMonitoring(builder.Configuration, builder.Environment);

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

app.MapControllers();

app.Run();

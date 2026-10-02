using Messaging.RabbitMQ;
using NewsletterService.Clients;
using NewsletterService.Workers;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddClients(builder.Configuration);
builder.Services.AddHostedService<ArticlePublishedWorker>();
builder.Services.AddMessaging(builder.Configuration);

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

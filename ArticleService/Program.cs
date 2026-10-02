using System.Text.Json.Serialization;
using ArticleService.Persistence;
using ArticleService.Services;
using ArticleService.Workers;
using Messaging.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddScoped<IArticleService, ArticleService.Services.ArticleService>();
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

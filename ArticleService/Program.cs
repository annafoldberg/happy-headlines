using System.Text.Json.Serialization;
using ArticleService.Caching;
using ArticleService.Persistence;
using ArticleService.Services;
using ArticleService.Workers;
using Messaging.RabbitMQ;
using Monitoring;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCaching(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddScoped<IArticleService, ArticleService.Services.ArticleService>();

builder.Services.AddHostedService<ArticleCacheRefreshWorker>();
builder.Services.AddHostedService<ArticlePublishedWorker>();

builder.Services.AddMessaging(builder.Configuration);
builder.Services.AddMonitoring(builder.Configuration, builder.Environment);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()));

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapPrometheusScrapingEndpoint();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

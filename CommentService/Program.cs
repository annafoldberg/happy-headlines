using CommentService.Caching;
using CommentService.Clients;
using CommentService.Persistence;
using CommentService.Services;
using Monitoring;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddClients(builder.Configuration);
builder.Services.AddScoped<ICommentService, CommentService.Services.CommentService>();
builder.Services.AddCaching(builder.Configuration);
builder.Services.AddMonitoring(builder.Configuration, builder.Environment);

builder.Services.AddControllers();

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

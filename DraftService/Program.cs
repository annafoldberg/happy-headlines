using System.Text.Json.Serialization;
using DraftService.Persistence;
using DraftService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddScoped<IDraftService, DraftService.Services.DraftService>();

builder.Services.AddControllers();

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

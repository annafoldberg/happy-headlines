using DraftService.Persistence;
using DraftService.Services;
using Monitoring;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddScoped<IDraftService, DraftService.Services.DraftService>();
builder.Services.AddMonitoring(builder.Configuration, builder.Environment);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();    

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

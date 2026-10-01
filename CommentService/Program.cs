using CommentService.Clients;
using CommentService.Persistence;
using CommentService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddClients(builder.Configuration);
builder.Services.AddScoped<ICommentService, CommentService.Services.CommentService>();

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

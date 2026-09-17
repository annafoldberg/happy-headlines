using System.Text.Json.Serialization;
using ArticleService.Persistence.Configuration;
using ArticleService.Persistence.Context;
using ArticleService.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configure persistence
builder.Services
    .AddOptions<DatabaseOptions>()
    .Bind(builder.Configuration.GetSection(DatabaseOptions.SectionName))
    .Validate(options =>
        !string.IsNullOrWhiteSpace(options.Africa.Host) &&
        !string.IsNullOrWhiteSpace(options.Antarctica.Host) &&
        !string.IsNullOrWhiteSpace(options.Asia.Host) &&
        !string.IsNullOrWhiteSpace(options.Europe.Host) &&
        !string.IsNullOrWhiteSpace(options.NorthAmerica.Host) &&
        !string.IsNullOrWhiteSpace(options.Oceania.Host) &&
        !string.IsNullOrWhiteSpace(options.SouthAmerica.Host) &&
        !string.IsNullOrWhiteSpace(options.Global.Host) &&
        !string.IsNullOrWhiteSpace(options.Name) &&
        !string.IsNullOrWhiteSpace(options.User) &&
        !string.IsNullOrWhiteSpace(options.Password),
        "Database configuration is incomplete.")
    .ValidateOnStart();

// Register dependencies
builder.Services.AddSingleton<IArticleDbContextFactory, ArticleDbContextFactory>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();

// Configure API
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()));;

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();    

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

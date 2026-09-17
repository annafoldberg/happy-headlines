using ArticleService.Persistence.Configuration;
using ArticleService.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ArticleService.Persistence.Context;

/// <summary>
/// Creates a database context for the relevant continent database.
/// </summary>
public sealed class ArticleDbContextFactory : IArticleDbContextFactory
{
    private readonly DatabaseOptions _options;

    public ArticleDbContextFactory(IOptions<DatabaseOptions> options)
    {
        _options = options.Value;
    }

    public ArticleDbContext Create(Continent continent)
    {
        var host = continent switch
        {
            Continent.Africa => _options.Africa.Host,
            Continent.Antarctica => _options.Antarctica.Host,
            Continent.Asia => _options.Asia.Host,
            Continent.Europe => _options.Europe.Host,
            Continent.NorthAmerica => _options.NorthAmerica.Host,
            Continent.Oceania => _options.Oceania.Host,
            Continent.SouthAmerica => _options.SouthAmerica.Host,
            Continent.Global => _options.Global.Host,
            _ => throw new ArgumentOutOfRangeException(nameof(continent))
        };

        var connectionString =
            $"Host={host};" +
            $"Database={_options.Name};" +
            $"Username={_options.User};" +
            $"Password={_options.Password}";

        var options = new DbContextOptionsBuilder<ArticleDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new ArticleDbContext(options);
    }
}
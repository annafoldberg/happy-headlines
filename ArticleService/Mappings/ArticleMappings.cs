using ArticleService.Contracts;
using ArticleService.Entities;
using Messaging.MessageContracts;
using ArticleContinent = ArticleService.Routing.Continent;
using MessagingContinent = Messaging.MessageContracts.Continent;


namespace ArticleService.Mappings;

/// <summary>
/// Maps article contracts and entities.
/// </summary>
public static class ArticleMappings
{
    public static ArticleResponse ToResponse(this Article article)
    {
        return new ArticleResponse
        {
            Id = article.PublicId,
            Author = article.Author,
            Title = article.Title,
            Content = article.Content,
            PublicationTimestampUtc = article.PublicationTimestampUtc,
            LastUpdatedTimestampUtc = article.LastUpdatedTimestampUtc
        };
    }

    public static Article ToEntity(this CreateArticleRequest request)
    {
        return new Article
        {
            Author = request.Author,
            Title = request.Title,
            Content = request.Content
        };
    }

    public static Article ToEntity(this ArticlePublished articlePublished)
    {
        return new Article
        {
            PublicId = articlePublished.ArticleId,
            Author = articlePublished.Author,
            Title = articlePublished.Title,
            Content = articlePublished.Content,
            PublicationTimestampUtc = articlePublished.PublicationTimestampUtc
        };
    }

    public static ArticleContinent ToArticleContinent(this MessagingContinent continent)
    {
        return continent switch
        {
            MessagingContinent.Africa => ArticleContinent.Africa,
            MessagingContinent.Antarctica => ArticleContinent.Antarctica,
            MessagingContinent.Asia => ArticleContinent.Asia,
            MessagingContinent.Europe => ArticleContinent.Europe,
            MessagingContinent.NorthAmerica => ArticleContinent.NorthAmerica,
            MessagingContinent.Oceania => ArticleContinent.Oceania,
            MessagingContinent.SouthAmerica => ArticleContinent.SouthAmerica,
            MessagingContinent.Global => ArticleContinent.Global,
            _ => throw new ArgumentOutOfRangeException(nameof(continent), continent, "Unsupported continent.")
        };
    }
}
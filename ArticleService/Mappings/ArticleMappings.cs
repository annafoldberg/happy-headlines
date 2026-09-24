using ArticleService.Contracts;
using ArticleService.Entities;

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
}
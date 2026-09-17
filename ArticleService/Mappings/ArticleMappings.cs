using ArticleService.Dtos;
using ArticleService.Entities;

namespace ArticleService.Mappings;

/// <summary>
/// Maps article DTOs and entities.
/// </summary>
public static class ArticleMappings
{
    public static ArticleDto ToDto(this Article article)
    {
        return new ArticleDto
        {
            Id = article.PublicId,
            PublicationTimestampUtc = article.PublicationTimestampUtc,
            LastUpdatedTimestampUtc = article.LastUpdatedTimestampUtc,
            Author = article.Author,
            Title = article.Title,
            Content = article.Content
        };
    }

    public static Article ToEntity(this CreateArticleDto createArticleDto)
    {
        return new Article
        {
            Author = createArticleDto.Author,
            Title = createArticleDto.Title,
            Content = createArticleDto.Content
        };
    }
}
using CommentService.Contracts;
using CommentService.Entities;

namespace CommentService.Mappings;

/// <summary>
/// Maps comment contracts and entities.
/// </summary>
public static class CommentMappings
{
    public static CommentResponse ToResponse(this Comment comment)
    {
        return new CommentResponse
        {
            Id = comment.PublicId,
            ArticleId = comment.ArticleId,
            Author = comment.Author,
            Content = comment.Content,
            CreationTimestampUtc = comment.CreationTimestampUtc,
            LastUpdatedTimestampUtc = comment.LastUpdatedTimestampUtc
        };
    }

    public static Comment ToEntity(this CreateCommentRequest request, Guid articleId, string content)
    {
        return new Comment
        {
            ArticleId = articleId,
            Author = request.Author,
            Content = content
        };
    }
}
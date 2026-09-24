using CommentService.Contracts;
using CommentService.Entities;

namespace CommentService.Mappings;

/// <summary>
/// Maps comment data to profanity filter contracts.
/// </summary>
public static class FilterCommentMappings
{
    public static FilterCommentRequest ToFilterRequest(this CreateCommentRequest request)
    {
        return new FilterCommentRequest
        {
            Comment = request.Content
        };
    }
}
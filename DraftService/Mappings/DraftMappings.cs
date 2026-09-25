using DraftService.Contracts;
using DraftService.Entities;

namespace DraftService.Mappings;

public static class DraftMappings
{
    public static DraftResponse ToResponse(this Draft draft)
    {
        return new DraftResponse
        {
            Id = draft.PublicId,
            Author = draft.Author,
            Title = draft.Title,
            Content = draft.Content,
            CreationTimestampUtc = draft.CreationTimestampUtc,
            LastUpdatedTimestampUtc = draft.LastUpdatedTimestampUtc
        };
    }

    public static Draft ToEntity(this DraftRequest request)
    {
        return new Draft
        {
            Author = request.Author,
            Title = request.Title,
            Content = request.Content
        };
    }
}
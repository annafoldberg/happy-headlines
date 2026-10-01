using Messaging.MessageContracts;
using PublisherService.Contracts;
using PublisherContinent = PublisherService.Contracts.Continent;
using MessagingContinent = Messaging.MessageContracts.Continent;

namespace PublisherService.Mappings;

public static class PublisherMappings
{
    public static ArticlePublished ToArticlePublished(this CreatePublicationRequest request)
    {
        return new ArticlePublished(
            ArticleId: Guid.NewGuid(),
            Author: request.Author,
            Title: request.Title,
            Content: request.Content,
            PublicationTimestampUtc: DateTime.UtcNow,
            Continent: request.Continent.ToMessageContinent()
        );
    }

    private static MessagingContinent ToMessageContinent(this PublisherContinent continent)
    {
        return continent switch
        {
            PublisherContinent.Africa => MessagingContinent.Africa,
            PublisherContinent.Antarctica => MessagingContinent.Antarctica,
            PublisherContinent.Asia => MessagingContinent.Asia,
            PublisherContinent.Europe => MessagingContinent.Europe,
            PublisherContinent.NorthAmerica => MessagingContinent.NorthAmerica,
            PublisherContinent.Oceania => MessagingContinent.Oceania,
            PublisherContinent.SouthAmerica => MessagingContinent.SouthAmerica,
            PublisherContinent.Global => MessagingContinent.Global,
            _ => throw new ArgumentOutOfRangeException(nameof(continent), continent, "Unsupported continent.")
        };
    }
}
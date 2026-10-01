namespace Messaging.MessageContracts;

/// <summary>
/// Event published when an article is published.
/// Produced by Publisher Service, consumed by Article Service and Newsletter Service.
/// </summary>
public sealed record ArticlePublished(Guid ArticleId, string Author, string Title, string Content, DateTime PublicationTimestampUtc, Continent Continent);
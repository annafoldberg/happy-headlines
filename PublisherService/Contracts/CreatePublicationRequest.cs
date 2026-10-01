using System.ComponentModel.DataAnnotations;

namespace PublisherService.Contracts;

public sealed class CreatePublicationRequest
{
    [Required]
    public string Author { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    [Required]
    public Continent Continent { get; set; }
}
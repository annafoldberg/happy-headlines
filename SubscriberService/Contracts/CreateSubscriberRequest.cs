using System.ComponentModel.DataAnnotations;

namespace SubscriberService.Contracts;

public class CreateSubscriberRequest
{
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
}
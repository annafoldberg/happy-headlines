using Microsoft.AspNetCore.Mvc;
using NewsletterService.Contracts;
using NewsletterService.Services;

namespace NewsletterService.Controllers;

[ApiController]
[Route("[controller]/{continent}")]
public class NewslettersController : ControllerBase
{
    private readonly INewsletterService _service;

    public NewslettersController(INewsletterService service)
    {
        _service = service;
    }

    [HttpGet("daily")]
    public async Task<IActionResult> GetDailyNewsletter([FromRoute] Continent continent,
        [FromQuery] DateOnly date, CancellationToken ct)
    {
        var newsletter = await _service.GetDailyNewsletterAsync(continent, date, ct);

        if (newsletter is null) return NoContent();

        return Ok(newsletter);
    }
}
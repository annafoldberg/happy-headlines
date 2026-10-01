using Microsoft.AspNetCore.Mvc;
using PublisherService.Contracts;
using PublisherService.Services;

namespace PublisherService.Controllers;

[ApiController]
[Route("[controller]")]
public class PublicationsController : ControllerBase
{
    private readonly IPublisherService _service;

    public PublicationsController(IPublisherService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Publish([FromBody] CreatePublicationRequest request, CancellationToken ct)
    {
        await _service.PublishArticleAsync(request, ct);

        return Accepted();
    }
}

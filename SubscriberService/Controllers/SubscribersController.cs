using SubscriberService.Contracts;
using SubscriberService.Services;
using Microsoft.AspNetCore.Mvc;

namespace SubscriberService.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscribersController : ControllerBase
{
    private readonly ISubscriberService _service;

    public SubscribersController(ISubscriberService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscriber([FromBody] CreateSubscriberRequest request, CancellationToken ct)
    {
        var (result, subscriber) = await _service.CreateSubscriberAsync(request, ct);

        return result switch
        {
            CreateSubscriberOperationResult.AlreadyExists =>
                Conflict("A subscriber with this email already exists."),

            CreateSubscriberOperationResult.Success when subscriber is not null =>
                StatusCode(StatusCodes.Status201Created, subscriber),

            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    [HttpGet]
    public async Task<IActionResult> GetSubscribers(CancellationToken ct)
    {
        var subscribers = await _service.GetSubscribersAsync(ct);

        return Ok(subscribers);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubscriber([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _service.DeleteSubscriberAsync(id, ct);

        if (result == DeleteSubscriberOperationResult.NotFound) return NotFound();

        return NoContent();
    }
}

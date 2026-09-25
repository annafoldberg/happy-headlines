using DraftService.Contracts;
using DraftService.Services;
using Microsoft.AspNetCore.Mvc;

namespace DraftService.Controllers;

[ApiController]
[Route("[controller]")]
public class DraftsController : ControllerBase
{
    private readonly IDraftService _service;

    public DraftsController(IDraftService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDraft([FromBody] DraftRequest request, CancellationToken ct)
    {
        var draft = await _service.CreateDraftAsync(request, ct);

        return CreatedAtAction(
            nameof(GetDraft),
            new { id = draft.Id },
            draft);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDraft([FromRoute] Guid id, CancellationToken ct)
    {
        var draft = await _service.GetDraftAsync(id, ct);

        if (draft is null) return NotFound();

        return Ok(draft);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDraft([FromRoute] Guid id, [FromBody] DraftRequest request, CancellationToken ct)
    {
        var result = await _service.UpdateDraftAsync(id, request, ct);

        if (result == DraftOperationResult.NotFound) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDraft([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _service.DeleteDraftAsync(id, ct);

        if (result == DraftOperationResult.NotFound) return NotFound();

        return NoContent();
    }
}

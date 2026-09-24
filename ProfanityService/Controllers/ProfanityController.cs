using Microsoft.AspNetCore.Mvc;
using ProfanityService.Contracts;
using ProfanityService.Services;

namespace ProfanityService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfanitiesController : ControllerBase
{
    private readonly IProfanityFilterService _filterService;

    public ProfanitiesController(IProfanityFilterService filterService)
    {
        _filterService = filterService;
    }

    [HttpPost("filter")]
    public async Task<IActionResult> FilterComment(
        [FromBody] FilterCommentRequest request,
        CancellationToken ct)
    {
        var response =  await _filterService.FilterCommentAsync(request, ct);

        return Ok(response);
    }
}
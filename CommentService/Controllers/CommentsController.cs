using CommentService.Contracts;
using CommentService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommentService.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController: ControllerBase
{
    private readonly ICommentService _service;

    public CommentsController(ICommentService service)
    {
        _service = service;
    }

    [HttpPost("{articleId}")]
    public async Task<IActionResult> CreateComment([FromRoute] Guid articleId, [FromBody] CreateCommentRequest request, CancellationToken ct)
    {
        var comment = await _service.CreateCommentAsync(articleId, request, ct);

        return CreatedAtAction(
            nameof(GetComment),
            new { id = comment.Id },
            comment);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetComment([FromRoute] Guid id, CancellationToken ct)
    {
        var comment = await _service.GetCommentAsync(id, ct);

        if (comment is null) return NotFound();

        return Ok(comment);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateArticle([FromRoute] Guid id, UpdateCommentRequest request, CancellationToken ct)
    {
        var result = await _service.UpdateCommentAsync(id, request, ct);

        if (result == CommentOperationResult.NotFound) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArticle([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _service.DeleteCommentAsync(id, ct);

        if (result == CommentOperationResult.NotFound) return NotFound();

        return NoContent();
    }
}
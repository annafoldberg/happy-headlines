using Microsoft.AspNetCore.Mvc;
using ArticleService.Contracts;
using ArticleService.Routing;
using ArticleService.Services;

namespace ArticleService.Controllers;

[ApiController]
[Route("[controller]/{continent}")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _service;

    public ArticlesController(IArticleService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateArticle([FromRoute] Continent continent,
        [FromBody] CreateArticleRequest request, CancellationToken ct)
    {
        var article = await _service.CreateArticleAsync(continent, request, ct);

        return CreatedAtAction(
            nameof(GetArticleById),
            new { continent, id = article.Id },
            article);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetArticleById([FromRoute] Continent continent, [FromRoute] Guid id, CancellationToken ct)
    {
        var article = await _service.GetArticleByIdAsync(continent, id, ct);

        if (article is null) return NotFound();

        return Ok(article);
    }

    [HttpGet]
    public async Task<IActionResult> GetArticleByDate(
        [FromRoute] Continent continent,
        [FromQuery] DateOnly publicationDate,
        CancellationToken ct)
    {
        var article = await _service.GetArticleByDateAsync(continent, publicationDate, ct);

        if (article is null) return NotFound();

        return Ok(article);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateArticle([FromRoute] Continent continent,
        [FromRoute] Guid id, [FromBody] UpdateArticleRequest request, CancellationToken ct)
    {
        var result = await _service.UpdateArticleAsync(continent, id, request, ct);

        if (result == ArticleOperationResult.NotFound) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArticle([FromRoute] Continent continent, [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await _service.DeleteArticleAsync(continent, id, ct);
        
        if (result == ArticleOperationResult.NotFound) return NotFound();

        return NoContent();
    }
}
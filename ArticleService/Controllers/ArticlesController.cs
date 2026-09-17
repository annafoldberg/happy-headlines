using Microsoft.AspNetCore.Mvc;
using ArticleService.Persistence.Repositories;
using ArticleService.Mappings;
using ArticleService.Dtos;
using ArticleService.Routing;

namespace ArticleService.Controllers;

[ApiController]
[Route("[controller]/{continent}")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleRepository _repository;

    public ArticlesController(IArticleRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromRoute] Continent continent,
        [FromBody] CreateArticleDto createArticleDto, CancellationToken ct)
    {
        var article = createArticleDto.ToEntity();
        await _repository.AddAsync(continent, article, ct);

        return CreatedAtAction(
            "GetById",
            new { continent, id = article.PublicId },
            article.ToDto());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleDto>> GetByIdAsync([FromRoute] Continent continent, [FromRoute] Guid id, CancellationToken ct)
    {
        var article = await _repository.GetByIdAsync(continent, id, ct);
        if (article is null) return NotFound();

        return Ok(article.ToDto());
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateAsync([FromRoute] Continent continent,
        [FromRoute] Guid id, [FromBody] UpdateArticleDto updateArticleDto, CancellationToken ct)
    {
        var article = await _repository.GetByIdAsync(continent, id, ct);
        if (article is null) return NotFound();

        var articleChanged =
            (updateArticleDto.Author is not null && updateArticleDto.Author != article.Author) ||
            (updateArticleDto.Title is not null && updateArticleDto.Title != article.Title) ||
            (updateArticleDto.Content is not null && updateArticleDto.Content != article.Content);

        if (articleChanged)
        {
            article.Author = updateArticleDto.Author ?? article.Author;
            article.Title = updateArticleDto.Title ?? article.Title;
            article.Content = updateArticleDto.Content ?? article.Content;
            
            article.LastUpdatedTimestampUtc = DateTime.UtcNow;
            await _repository.UpdateAsync(continent, article, ct);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] Continent continent, [FromRoute] Guid id,
        CancellationToken ct)
    {
        var article = await _repository.GetByIdAsync(continent, id, ct);
        if (article is null) return NotFound();

        await _repository.DeleteAsync(continent, article, ct);

        return NoContent();
    }
}
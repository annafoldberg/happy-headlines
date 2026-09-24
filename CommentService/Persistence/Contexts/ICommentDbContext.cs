using CommentService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Persistence.Contexts;

/// <summary>
/// Defines the comment database context.
/// </summary>
public interface ICommentDbContext
{
    DbSet<Comment> Comments { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
using CommentService.Entities;
using CommentService.Persistence.Contexts;
using CommentService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ICommentDbContext _context;

    public CommentRepository(ICommentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Comment comment, CancellationToken ct)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<Comment?> GetByPublicIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Comments.FirstOrDefaultAsync(c => c.PublicId == id, ct);
    }

    public async Task UpdateAsync(Comment comment, CancellationToken ct)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Comment comment, CancellationToken ct)
    {
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }
}
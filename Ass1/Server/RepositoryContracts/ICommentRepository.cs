using Entities;

namespace RepositoryContracts;

public interface ICommentRepository
{
    Task<Comment> AddAsync(Comment comment);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(Guid commentId);
    Task<Comment> GetSingleAsync(Guid id);
    Task<List<Comment>> GetAllAsync(Guid postId);
    IQueryable<Comment> GetMany();
}

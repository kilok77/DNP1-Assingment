using Entities;

namespace RepositoryContracts;

public interface ICommentRepository
{
    Task<Comment> AddAsync(Comment comment);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(Guid commentId);
    Task<Comment> GetSingleAsync(Guid id);
    IQueryable<Comment> GetMany();
}

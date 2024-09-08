using Entities;

namespace RepositoryContracts;

public interface IPostRepository
{
    Task<Post> AddAsync(Post post);
    Task UpdateAsync(Post post);
    Task DeleteAsync(Guid post);
    Task<Post> GetSingleAsync(Guid id);
    IQueryable<Post> GetManyAsync();
}
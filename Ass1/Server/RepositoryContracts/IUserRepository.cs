using Entities;

namespace RepositoryContracts
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid userId);
        Task<User> GetSingleAsync(Guid id);
        IQueryable<User> GetMany();
    }
}
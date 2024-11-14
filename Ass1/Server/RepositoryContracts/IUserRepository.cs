using Entities;

namespace RepositoryContracts
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid userId);
        IQueryable<User> GetMany();
        Task<User> GetSingleAsync(Guid id);
        Task<User> GetSingleAsync(string email);
        
        Guid GetCurrentUserId();
    }
}
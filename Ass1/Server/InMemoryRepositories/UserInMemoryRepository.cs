using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository
{
    private readonly List<User> users;
    
    private readonly Guid currentUserId;

    public UserInMemoryRepository()
    {
        users = new List<User>();
        InitialDummyData();
        currentUserId = new Guid("11111111-1111-1111-1111-111111111111");
    }
    
    private void InitialDummyData()
    {
        

    }

    public Guid GetCurrentUserId() => currentUserId;

    public Task<User> AddAsync(User user)
    {
        user.Id = Guid.NewGuid();
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        var existingUser = users.SingleOrDefault(x => x.Id == user.Id);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"User with ID '{user.Id}' was not found");
        }

        users.Remove(existingUser);
        users.Add(user);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid userId)
    {
        var userToRemove = users.SingleOrDefault(x => x.Id == userId);
        if (userToRemove == null)
        {
            throw new InvalidOperationException($"User with ID '{userId}' was not found");
        }

        users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public async Task<User> GetSingleAsync(Guid userId)
    {
        var user = users.SingleOrDefault(x => x.Id == userId);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID '{userId}' was not found");
        }

        return user;
    }

    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
}
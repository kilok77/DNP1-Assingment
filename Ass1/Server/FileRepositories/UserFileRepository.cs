using Entities;
using RepositoryContracts;
using System.Text.Json;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<User> AddAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        users.Add(user);

        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, usersAsJson);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        int index = users.FindIndex(u => u.UserId == user.UserId);
        if (index != -1)
        {
            users[index] = user;
            usersAsJson = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(filePath, usersAsJson);
        }
        else
        {
            throw new KeyNotFoundException("User not found.");
        }
    }

    public async Task DeleteAsync(Guid userId)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        var userToDelete = users.FirstOrDefault(u => u.UserId == userId);
        if (userToDelete != null)
        {
            users.Remove(userToDelete);
            usersAsJson = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(filePath, usersAsJson);
        }
        else
        {
            throw new KeyNotFoundException("User not found.");
        }
    }

    public async Task<User> GetSingleAsync(Guid id)
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        var user = users.FirstOrDefault(u => u.UserId == id);
        return user ?? throw new KeyNotFoundException("User not found.");
    }

    public IQueryable<User> GetMany()
    {
        string usersAsJson = File.ReadAllText(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users.AsQueryable();
    }

    public Guid GetCurrentUserId()
    {
        // Implement logic to get the current user ID
        throw new NotImplementedException();
    }
}

using Entities;
using RepositoryContracts;
using System.Text.Json;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
 
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        post.CreatedAt = DateTime.UtcNow;
        posts.Add(post);
        
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        int index = posts.FindIndex(p => p.PostId == post.PostId);
        if (index != -1)
        {
            posts[index] = post;
            postsAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(filePath, postsAsJson);
        }
        else
        {
            throw new KeyNotFoundException("Post not found.");
        }
    }

    public async Task DeleteAsync(Guid postId)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        var postToDelete = posts.FirstOrDefault(p => p.PostId == postId);
        if (postToDelete != null)
        {
            posts.Remove(postToDelete);
            postsAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(filePath, postsAsJson);
        }
        else
        {
            throw new KeyNotFoundException("Post not found.");
        }
    }

    public async Task<Post> GetSingleAsync(Guid id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        var post = posts.FirstOrDefault(p => p.PostId == id);
        return post ?? throw new KeyNotFoundException("Post not found.");
    }

    public IQueryable<Post> GetManyAsync()
    {
        string postsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.AsQueryable();
    }
}

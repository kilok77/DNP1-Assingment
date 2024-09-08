using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    List<Post> posts;
    
    public PostInMemoryRepository()
    {
        posts = new List<Post>();
        InitialDummyData();
    }
    
    private void InitialDummyData()
    {
        var userId = new Guid("11111111-1111-1111-1111-111111111111"); // Fixed user ID

        posts.Add(new Post
        {
            PostId = new Guid("22222222-2222-2222-2222-222222222222"),
            Title = "First Post",
            Content = "This is the content of the first post.",
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        });

        posts.Add(new Post
        {
            PostId = new Guid("33333333-3333-3333-3333-333333333333"),
            Title = "Second Post",
            Content = "This is the content of the second post.",
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        });
    }


    public Task<Post> AddAsync(Post post)
    {
        post.PostId = Guid.NewGuid();
        
        posts.Add(post);
        return Task.FromResult(post);
    }
    
    

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault((x) => x.PostId == post.PostId);
        if (existingPost == null)
        {
            throw new InvalidOperationException($"Post with ID '{post.PostId}' was not found");
        }
        
        posts.Remove(existingPost);
        posts.Add(post);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid postId)
    {
        Post? postToRemove = posts.SingleOrDefault((x) => x.PostId == postId);
        if (postToRemove == null)
        {
            throw new InvalidOperationException($"Post with ID '{postId}' was not found");
        }
        
        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(Guid postId)
    {
        Post? post = posts.SingleOrDefault((x) => x.PostId == postId);
        if (post == null)
        {
            throw new InvalidOperationException($"Post with ID '{postId}' was not found");
        }
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetManyAsync()
    {
        return posts.AsQueryable();
    }
}
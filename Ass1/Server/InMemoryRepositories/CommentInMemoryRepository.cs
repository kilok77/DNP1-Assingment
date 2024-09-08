using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private readonly List<Comment> comments;

    public CommentInMemoryRepository()
    {
        comments = new List<Comment>();
        InitialDummyData();
    }
    
    private void InitialDummyData()
    {
        var postId = new Guid("22222222-2222-2222-2222-222222222222"); // Use an existing post ID
        var userId = new Guid("11111111-1111-1111-1111-111111111111"); // Use an existing user ID

        comments.Add(new Comment
        {
            CommentId = new Guid("55555555-5555-5555-5555-555555555555"),
            UserId = userId,
            PostId = postId,
            Content = "This is the first comment.",
            CreatedAt = DateTime.UtcNow
        });

        comments.Add(new Comment
        {
            CommentId = new Guid("66666666-6666-6666-6666-666666666666"),
            UserId = userId,
            PostId = postId,
            Content = "This is the second comment.",
            CreatedAt = DateTime.UtcNow
        });
    }


    public Task<Comment> AddAsync(Comment comment)
    {
        comment.CommentId = Guid.NewGuid();
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        var existingComment = comments.SingleOrDefault(x => x.CommentId == comment.CommentId);
        if (existingComment == null)
        {
            throw new InvalidOperationException($"Comment with ID '{comment.CommentId}' was not found");
        }

        comments.Remove(existingComment);
        comments.Add(comment);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid commentId)
    {
        var commentToRemove = comments.SingleOrDefault(x => x.CommentId == commentId);
        if (commentToRemove == null)
        {
            throw new InvalidOperationException($"Comment with ID '{commentId}' was not found");
        }

        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(Guid commentId)
    {
        var comment = comments.SingleOrDefault(x => x.CommentId == commentId);
        if (comment == null)
        {
            throw new InvalidOperationException($"Comment with ID '{commentId}' was not found");
        }

        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}
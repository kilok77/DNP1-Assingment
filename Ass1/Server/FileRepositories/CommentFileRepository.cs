using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{

    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    public async Task<Comment> AddAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        comments.Add(comment);
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        // Find the index of the existing comment
        int index = comments.FindIndex(c => c.CommentId == comment.CommentId);

        if (index != -1)
        {
            // Replace the existing comment with the updated one
            comments[index] = comment;
        
            // Write back the updated list to the file
            commentsAsJson = JsonSerializer.Serialize(comments);
            await File.WriteAllTextAsync(filePath, commentsAsJson);
        }
        else
        {
            throw new KeyNotFoundException("Comment not found.");
        }
    }


    public async Task DeleteAsync(Guid commentId)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        var commentToDelete = comments.FirstOrDefault(c => c.CommentId == commentId);
        if (commentToDelete != null)
        {
            comments.Remove(commentToDelete);

            // Write the updated list back to the file
            commentsAsJson = JsonSerializer.Serialize(comments);
            await File.WriteAllTextAsync(filePath, commentsAsJson);
        }
        else
        {
            throw new KeyNotFoundException("Comment not found.");
        }
    }

    public async Task<Comment> GetSingleAsync(Guid id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        var comment = comments.FirstOrDefault(c => c.CommentId == id);
        if (comment != null)
        {
            return comment;
        }
        else
        {
            throw new KeyNotFoundException("Comment not found.");
        }
    }

    public IQueryable<Comment> GetMany()
    {
        string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        return comments.AsQueryable();
    }
}
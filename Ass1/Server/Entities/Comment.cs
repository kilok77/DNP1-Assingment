namespace Entities;

public class Comment
{
    public Guid CommentId { get; set; }
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
    
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}
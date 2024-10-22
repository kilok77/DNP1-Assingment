namespace ApiContracts.CommentDto;

public class CreateCommentDto
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
}

public class UpdateCommentDto
{
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
}

public class CommentDto
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}

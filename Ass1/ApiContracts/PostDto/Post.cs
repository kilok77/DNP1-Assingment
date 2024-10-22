namespace ApiContracts.PostDto;

public class CreatePostDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid UserId { get; set; }
}

public class UpdatePostDto
{
    public string Title { get; set; }
    public string Content { get; set; }
}

public class PostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}

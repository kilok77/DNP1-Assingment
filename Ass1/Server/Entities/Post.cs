using System.Runtime.InteropServices.JavaScript;

namespace Entities;

public class Post
{
    public Guid PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    
}
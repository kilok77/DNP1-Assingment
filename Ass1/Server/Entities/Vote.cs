namespace Entities;

public class Vote
{
    public Guid VoteId { get; set; }
    public Guid CommentId { get; set; }
    public int IsUpVote { get; set; }
    public DateTime CreatedAt { get; set; }
}
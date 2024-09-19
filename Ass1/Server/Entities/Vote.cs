namespace Entities;

public enum VoteType
{
    Post,
    Comment
}

public class Vote
{
    public Guid VoteId { get; set; }
    public Guid EntityId { get; set; } // This can be either PostId or CommentId
    public VoteType EntityType { get; set; } // Indicates whether the vote is for a Post or a Comment
    public Guid UserId { get; set; }
    public bool IsUpVote { get; set; } // true for upvote, false for downvote
    public DateTime CreatedAt { get; set; }
}
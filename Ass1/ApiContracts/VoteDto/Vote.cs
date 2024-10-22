using Entities;

namespace ApiContracts.VoteDto;

public class CreateVoteDto
{
    public Guid EntityId { get; set; } // This can be either PostId or CommentId
    public VoteType EntityType { get; set; } // Indicates whether the vote is for a Post or a Comment
    public Guid UserId { get; set; }
    public bool IsUpVote { get; set; } // true for upvote, false for downvote
}

public class UpdateVoteDto
{
    public Guid EntityId { get; set; } // This can be either PostId or CommentId
    public VoteType EntityType { get; set; } // Indicates whether the vote is for a Post or a Comment
    public Guid UserId { get; set; } // User who cast the vote
    public bool IsUpVote { get; set; } // Update whether the vote is an upvote or downvote
}

public class VoteDto
{
    public Guid VoteId { get; set; }
    public Guid EntityId { get; set; }
    public VoteType EntityType { get; set; }
    public Guid UserId { get; set; }
    public bool IsUpVote { get; set; }
    public DateTime CreatedAt { get; set; }
}

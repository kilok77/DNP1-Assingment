using RepositoryContracts;
using Entities;

namespace CLI.ManageVotes;

public class ManageVotes
{
    private readonly IVoteRepository voteRepository;
    private readonly IUserRepository userRepository;
    private readonly Guid entityId; // PostId or CommentId
    private readonly VoteType voteType; // Indicates whether it's a Post or Comment
    private readonly Guid userId; // Current user who is voting

    public ManageVotes(IVoteRepository voteRepository, IUserRepository userRepository, Guid entityId, VoteType voteType)
    {
        this.voteRepository = voteRepository;
        this.userRepository = userRepository;
        this.entityId = entityId;
        this.voteType = voteType;
        this.userId = userRepository.GetCurrentUserId();
    }

    public async Task DisplayAsync()
    {
        // Fetch votes for the entity
        IQueryable<Vote> votes = voteType == VoteType.Post
            ? voteRepository.GetVotesForPostAsync(entityId)
            : voteRepository.GetVotesForCommentAsync(entityId);

        var votesList = votes.ToList();

        // Calculate upvotes and downvotes
        int upvotes = votesList.Count(v => v.IsUpVote);
        int downvotes = votesList.Count(v => !v.IsUpVote);

        Console.WriteLine($"Entity {entityId} has {upvotes} upvotes and {downvotes} downvotes.");

        // Check if the current user has already voted
        var userVote = votesList.FirstOrDefault(v => v.UserId == userId);
        if (userVote != null)
        {
            Console.WriteLine("You have already voted on this entity.");
            Console.WriteLine(userVote.IsUpVote ? "Your vote: Upvote" : "Your vote: Downvote");
        }
        else
        {
            Console.WriteLine("You have not voted on this entity yet.");
            Console.WriteLine("1. Upvote");
            Console.WriteLine("2. Downvote");
            Console.WriteLine("3. Back");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await CastVoteAsync(true); // Upvote
                    break;
                case "2":
                    await CastVoteAsync(false); // Downvote
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private async Task CastVoteAsync(bool isUpVote)
    {
        var vote = new Vote
        {
            VoteId = Guid.NewGuid(),
            EntityId = entityId,
            EntityType = voteType,
            UserId = userId,
            IsUpVote = isUpVote,
            CreatedAt = DateTime.UtcNow
        };

        await voteRepository.AddAsync(vote);
        Console.WriteLine(isUpVote ? "You upvoted this entity." : "You downvoted this entity.");
    }
}

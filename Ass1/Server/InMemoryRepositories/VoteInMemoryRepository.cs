using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class VoteInMemoryRepository : IVoteRepository
{
    private readonly List<Vote> votes;

    public VoteInMemoryRepository()
    {
        votes = new List<Vote>();
        InitialDummyData();
    }
    
    private void InitialDummyData()
    {
        var commentId = new Guid("55555555-5555-5555-5555-555555555555"); // Use an existing comment ID

        votes.Add(new Vote
        {
            VoteId = new Guid("77777777-7777-7777-7777-777777777777"),
            CommentId = commentId,
            IsUpVote = 1,
            CreatedAt = DateTime.UtcNow
        });

        votes.Add(new Vote
        {
            VoteId = new Guid("88888888-8888-8888-8888-888888888888"),
            CommentId = commentId,
            IsUpVote = 0,
            CreatedAt = DateTime.UtcNow
        });
    }


    public Task<Vote> AddAsync(Vote vote)
    {
        vote.VoteId = Guid.NewGuid();
        votes.Add(vote);
        return Task.FromResult(vote);
    }

    public Task UpdateAsync(Vote vote)
    {
        var existingVote = votes.SingleOrDefault(x => x.VoteId == vote.VoteId);
        if (existingVote == null)
        {
            throw new InvalidOperationException($"Vote with ID '{vote.VoteId}' was not found");
        }

        votes.Remove(existingVote);
        votes.Add(vote);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid voteId)
    {
        var voteToRemove = votes.SingleOrDefault(x => x.VoteId == voteId);
        if (voteToRemove == null)
        {
            throw new InvalidOperationException($"Vote with ID '{voteId}' was not found");
        }

        votes.Remove(voteToRemove);
        return Task.CompletedTask;
    }

    public Task<Vote> GetSingleAsync(Guid voteId)
    {
        var vote = votes.SingleOrDefault(x => x.VoteId == voteId);
        if (vote == null)
        {
            throw new InvalidOperationException($"Vote with ID '{voteId}' was not found");
        }

        return Task.FromResult(vote);
    }

    public IQueryable<Vote> GetMany()
    {
        return votes.AsQueryable();
    }
}
using Entities;
using RepositoryContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryRepositories;

public class VoteInMemoryRepository : IVoteRepository
{
    private readonly List<Vote> votes = new List<Vote>();

    public Task<Vote> AddAsync(Vote vote)
    {
        votes.Add(vote);
        return Task.FromResult(vote);
    }

    public Task UpdateAsync(Vote vote)
    {
        var existingVote = votes.SingleOrDefault(v => v.VoteId == vote.VoteId);
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
        var vote = votes.SingleOrDefault(v => v.VoteId == voteId);
        if (vote == null)
        {
            throw new InvalidOperationException($"Vote with ID '{voteId}' was not found");
        }

        votes.Remove(vote);
        return Task.CompletedTask;
    }

    public Task<Vote> GetSingleAsync(Guid voteId)
    {
        var vote = votes.SingleOrDefault(v => v.VoteId == voteId);
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

    public IQueryable<Vote> GetVotesForPostAsync(Guid postId)
    {
        return votes.Where(v => v.EntityType == VoteType.Post && v.EntityId == postId).AsQueryable();
    }

    public IQueryable<Vote> GetVotesForCommentAsync(Guid commentId)
    {
        return votes.Where(v => v.EntityType == VoteType.Comment && v.EntityId == commentId).AsQueryable();
    }
}
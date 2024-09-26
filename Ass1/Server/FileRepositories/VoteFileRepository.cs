using Entities;
using RepositoryContracts;
using System.Text.Json;

namespace FileRepositories;

public class VoteFileRepository : IVoteRepository
{
    private readonly string filePath = "votes.json";

    public VoteFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Vote> AddAsync(Vote vote)
    {
        string votesAsJson = await File.ReadAllTextAsync(filePath);
        List<Vote> votes = JsonSerializer.Deserialize<List<Vote>>(votesAsJson)!;
        vote.CreatedAt = DateTime.UtcNow;
        votes.Add(vote);

        votesAsJson = JsonSerializer.Serialize(votes);
        await File.WriteAllTextAsync(filePath, votesAsJson);
        return vote;
    }

    public async Task UpdateAsync(Vote vote)
    {
        string votesAsJson = await File.ReadAllTextAsync(filePath);
        List<Vote> votes = JsonSerializer.Deserialize<List<Vote>>(votesAsJson)!;

        int index = votes.FindIndex(v => v.VoteId == vote.VoteId);
        if (index != -1)
        {
            votes[index] = vote;
            votesAsJson = JsonSerializer.Serialize(votes);
            await File.WriteAllTextAsync(filePath, votesAsJson);
        }
        else
        {
            throw new KeyNotFoundException("Vote not found.");
        }
    }

    public async Task DeleteAsync(Guid voteId)
    {
        string votesAsJson = await File.ReadAllTextAsync(filePath);
        List<Vote> votes = JsonSerializer.Deserialize<List<Vote>>(votesAsJson)!;

        var voteToDelete = votes.FirstOrDefault(v => v.VoteId == voteId);
        if (voteToDelete != null)
        {
            votes.Remove(voteToDelete);
            votesAsJson = JsonSerializer.Serialize(votes);
            await File.WriteAllTextAsync(filePath, votesAsJson);
        }
        else
        {
            throw new KeyNotFoundException("Vote not found.");
        }
    }

    public async Task<Vote> GetSingleAsync(Guid voteId)
    {
        string votesAsJson = await File.ReadAllTextAsync(filePath);
        List<Vote> votes = JsonSerializer.Deserialize<List<Vote>>(votesAsJson)!;

        var vote = votes.FirstOrDefault(v => v.VoteId == voteId);
        return vote ?? throw new KeyNotFoundException("Vote not found.");
    }

    public IQueryable<Vote> GetMany()
    {
        string votesAsJson = File.ReadAllText(filePath);
        List<Vote> votes = JsonSerializer.Deserialize<List<Vote>>(votesAsJson)!;
        return votes.AsQueryable();
    }

    public IQueryable<Vote> GetVotesForPostAsync(Guid postId)
    {
        string votesAsJson = File.ReadAllText(filePath);
        List<Vote> votes = JsonSerializer.Deserialize<List<Vote>>(votesAsJson)!;
        return votes.Where(v => v.EntityType == VoteType.Post && v.EntityId == postId).AsQueryable();
    }

    public IQueryable<Vote> GetVotesForCommentAsync(Guid commentId)
    {
        string votesAsJson = File.ReadAllText(filePath);
        List<Vote> votes = JsonSerializer.Deserialize<List<Vote>>(votesAsJson)!;
        return votes.Where(v => v.EntityType == VoteType.Comment && v.EntityId == commentId).AsQueryable();
    }
}

using Entities;

namespace RepositoryContracts;

public interface IVoteRepository
{
    Task<Vote> AddAsync(Vote vote);
    Task UpdateAsync(Vote vote);
    Task DeleteAsync(Guid voteId);
    Task<Vote> GetSingleAsync(Guid voteId);
    IQueryable<Vote> GetMany();
    IQueryable<Vote> GetVotesForPostAsync(Guid postId);
    IQueryable<Vote> GetVotesForCommentAsync(Guid commentId);
}
using ApiContracts;
using ApiContracts.VoteDto;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class VotesController : ControllerBase
{
    private readonly IVoteRepository voteRepository;

    public VotesController(IVoteRepository voteRepository)
    {
        this.voteRepository = voteRepository;
    }

    [HttpPost]
    public async Task<ActionResult<VoteDto>> CreateVoteAsync([FromBody] CreateVoteDto request)
    {
        Vote vote = new Vote
        {
            VoteId = Guid.NewGuid(),
            EntityId = request.EntityId,
            EntityType = request.EntityType,
            UserId = request.UserId,
            IsUpVote = request.IsUpVote,
            CreatedAt = DateTime.UtcNow
        };

        Vote created = await voteRepository.AddAsync(vote);
        VoteDto dto = new()
        {
            VoteId = created.VoteId,
            EntityId = created.EntityId,
            EntityType = created.EntityType,
            UserId = created.UserId,
            IsUpVote = created.IsUpVote,
            CreatedAt = created.CreatedAt
        };
        return Created($"votes/{dto.VoteId}", dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVoteAsync(Guid id, [FromBody] UpdateVoteDto request)
    {
        var vote = new Vote
        {
            VoteId = id,
            EntityId = request.EntityId,
            EntityType = request.EntityType,
            UserId = request.UserId,
            IsUpVote = request.IsUpVote
        };

        await voteRepository.UpdateAsync(vote);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVoteAsync(Guid id)
    {
        await voteRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VoteDto>> GetVoteAsync(Guid id)
    {
        var vote = await voteRepository.GetSingleAsync(id);
        if (vote == null)
        {
            return NotFound();
        }

        VoteDto dto = new()
        {
            VoteId = vote.VoteId,
            EntityId = vote.EntityId,
            EntityType = vote.EntityType,
            UserId = vote.UserId,
            IsUpVote = vote.IsUpVote,
            CreatedAt = vote.CreatedAt
        };
        return Ok(dto);
    }

    [HttpGet("post/{postId}")]
    public ActionResult<IEnumerable<VoteDto>> GetVotesForPostAsync(Guid postId)
    {
        var votes = voteRepository.GetVotesForPostAsync(postId).ToList();
        var voteDtos = votes.Select(v => new VoteDto
        {
            VoteId = v.VoteId,
            EntityId = v.EntityId,
            EntityType = v.EntityType,
            UserId = v.UserId,
            IsUpVote = v.IsUpVote,
            CreatedAt = v.CreatedAt
        });

        return Ok(voteDtos);
    }

    [HttpGet("comment/{commentId}")]
    public ActionResult<IEnumerable<VoteDto>> GetVotesForCommentAsync(Guid commentId)
    {
        var votes = voteRepository.GetVotesForCommentAsync(commentId).ToList();
        var voteDtos = votes.Select(v => new VoteDto
        {
            VoteId = v.VoteId,
            EntityId = v.EntityId,
            EntityType = v.EntityType,
            UserId = v.UserId,
            IsUpVote = v.IsUpVote,
            CreatedAt = v.CreatedAt
        });

        return Ok(voteDtos);
    }
}

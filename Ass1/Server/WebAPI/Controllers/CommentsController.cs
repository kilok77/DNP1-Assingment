using ApiContracts;
using ApiContracts.CommentDto;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateCommentAsync([FromBody] CreateCommentDto request)
    {
        Comment comment = new Comment
        {
            CommentId = Guid.NewGuid(),
            PostId = request.PostId,
            UserId = request.UserId,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        Comment created = await commentRepository.AddAsync(comment);
        CommentDto dto = new()
        {
            Id = created.CommentId,
            PostId = created.PostId,
            UserId = created.UserId,
            Content = created.Content,
            CreatedAt = created.CreatedAt
        };
        return Created($"comments/{dto.Id}", dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCommentAsync(Guid id, [FromBody] UpdateCommentDto request)
    {
        var comment = new Comment
        {
            CommentId = id,
            PostId = request.PostId,
            UserId = request.UserId,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow // Optionally update the timestamp
        };

        await commentRepository.UpdateAsync(comment);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCommentAsync(Guid id)
    {
        await commentRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetCommentAsync(Guid id)
    {
        var comment = await commentRepository.GetSingleAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        CommentDto dto = new()
        {
            Id = comment.CommentId,
            PostId = comment.PostId,
            UserId = comment.UserId,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt
        };
        return Ok(dto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetAllComments()
    {
        var comments = commentRepository.GetMany().ToList(); // Assuming GetMany returns IQueryable<Comment>
        var commentDtos = comments.Select(c => new CommentDto
        {
            Id = c.CommentId,
            PostId = c.PostId,
            UserId = c.UserId,
            Content = c.Content,
            CreatedAt = c.CreatedAt
        });

        return Ok(commentDtos);
    }
}

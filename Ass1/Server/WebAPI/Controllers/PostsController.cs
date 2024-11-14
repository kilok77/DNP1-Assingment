using ApiContracts;
using ApiContracts.CommentDto;
using ApiContracts.PostDto;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public PostsController(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePostAsync([FromBody] PostDto request)
    {
        Post post = new Post
        {
            PostId = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        };

        Post created = await postRepository.AddAsync(post);
        PostDto dto = new()
        {
            Id = created.PostId,
            Title = created.Title,
            Content = created.Content,
            UserId = created.UserId,
            CreatedAt = created.CreatedAt
        };
        return Created($"posts/{dto.Id}", dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePostAsync(Guid id, [FromBody] UpdatePostDto request)
    {
        var post = new Post
        {
            PostId = id,
            Title = request.Title,
            Content = request.Content
        };

        await postRepository.UpdateAsync(post);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePostAsync(Guid id)
    {
        await postRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPostAsync(Guid id)
    {
        var post = await postRepository.GetSingleAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        PostDto dto = new()
        {
            Id = post.PostId,
            Title = post.Title,
            Content = post.Content,
            UserId = post.UserId,
            CreatedAt = post.CreatedAt
        };
        return Ok(dto);
    }
    
    [HttpGet("{id}/comments")]
    public async Task<ActionResult<List<CommentDto>>> GetCommentsForPostAsync(Guid id)
    {
        Console.WriteLine(id);
        if (id == Guid.Empty)
        {
            return BadRequest("Post ID is invalid.");
        }

        try
        {
            var comments = await commentRepository.GetAllAsync(id);

            if (comments == null || !comments.Any())
            {
                return NotFound("No comments found for this post.");
            }

            var commentDtos = comments.Select(c => new CommentDto
            {
                Id = c.CommentId,
                PostId = c.PostId,
                UserId = c.UserId,
                Content = c.Content,
                CreatedAt = c.CreatedAt
            }).ToList();

            return Ok(commentDtos);
        }
        catch (Exception ex)
        {
            // Log the exception details
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }


    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetAllPosts()
    {
        var posts = postRepository.GetManyAsync().ToList();
        var postDtos = posts.Select(p => new PostDto
        {
            Id = p.PostId,
            Title = p.Title,
            Content = p.Content,
            UserId = p.UserId,
            CreatedAt = p.CreatedAt
        });

        return Ok(postDtos);
    }
}

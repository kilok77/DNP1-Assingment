using ApiContracts;
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

    public PostsController(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePostAsync([FromBody] CreatePostDto request)
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

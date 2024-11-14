using ApiContracts.PostDto;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDto> GetPostAsync(Guid postId); 
    public Task<List<PostDto>> GetAllPostsAsync(); 
    
    public Task<PostDto> AddPostAsync(PostDto postDto);
}

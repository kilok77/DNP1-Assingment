using System.Net.Http.Json;
using ApiContracts.PostDto;
using System.Text.Json;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<PostDto> GetPostAsync(Guid postId)
    {
        HttpResponseMessage httpResponse = await client.GetAsync($"posts/{postId}");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(
            response,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        )!;
    }

    public async Task<PostDto> AddPostAsync(PostDto createPostDto)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("posts", createPostDto);
        string response = await httpResponse.Content.ReadAsStringAsync();
        
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(
            response,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        )!;
    }

    public async Task<List<PostDto>> GetAllPostsAsync()
    {
        HttpResponseMessage httpResponse = await client.GetAsync("posts");
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<PostDto>>(
            response,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        )!;
    }
}
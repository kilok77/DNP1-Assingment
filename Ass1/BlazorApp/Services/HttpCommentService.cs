using ApiContracts.CommentDto;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlazorApp.Services
{
    public class HttpCommentService : ICommentService
    {
        private readonly HttpClient client;

        public HttpCommentService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<List<CommentDto>> GetComments(Guid postId)
        {
            try
            {
                // Send GET request to retrieve comments for the given postId
                HttpResponseMessage httpResponse = await client.GetAsync($"posts/{postId}/comments");
                
                // Ensure the request was successful
                httpResponse.EnsureSuccessStatusCode();

                // Deserialize the response content into a List of CommentDto
                var comments = await httpResponse.Content.ReadFromJsonAsync<List<CommentDto>>();
                Console.WriteLine(comments);
                return comments ?? new List<CommentDto>(); // return empty list if null
            }
            catch (Exception ex)
            {
                // Handle errors appropriately (e.g., log the error or rethrow)
                Console.WriteLine($"Error loading comments: {ex.Message}");
                return new List<CommentDto>(); // return empty list on failure
            }
        }

        public async Task<CommentDto> AddComment(CommentDto comment)
        {
            try
            {
                HttpResponseMessage httpResponse = await client.PostAsJsonAsync("comments/", comment);
                httpResponse.EnsureSuccessStatusCode();
                
                var commentDto = await httpResponse.Content.ReadFromJsonAsync<CommentDto>();
                
                return commentDto ?? new CommentDto(); // return empty list if null
                
            }
            catch (Exception ex)
            {
                // Handle errors appropriately (e.g., log the error or rethrow)
                Console.WriteLine($"Error adding comment: {ex.Message}");
                return new CommentDto(); // return empty list on failure
            }
        }
    }
}
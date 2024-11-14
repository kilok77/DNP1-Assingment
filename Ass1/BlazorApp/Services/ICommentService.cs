using ApiContracts.CommentDto;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<List<CommentDto>> GetComments(Guid postId);
    public Task<CommentDto> AddComment(CommentDto comment);
}

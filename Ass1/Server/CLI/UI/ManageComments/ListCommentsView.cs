using RepositoryContracts;

namespace CLI.ManageComments;

public class ListCommentsView
{
    private readonly ICommentRepository commentRepository;
    private readonly Guid postId;

    public ListCommentsView(ICommentRepository commentRepository, Guid postId)
    {
        this.commentRepository = commentRepository;
        this.postId = postId;
    }

    public void Display()
    {
        var comments = commentRepository.GetMany().Where(c => c.PostId == postId);

        if (!comments.Any())
        {
            Console.WriteLine("No comments available for this post.");
        }
        else
        {
            Console.WriteLine("List of comments:");
            foreach (var comment in comments)
            {
                Console.WriteLine($"Comment ID: {comment.CommentId}, User ID: {comment.UserId}, Content: {comment.Content}, Created At: {comment.CreatedAt}");
            }
        }
    }
}
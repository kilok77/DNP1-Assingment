using CLI.ManageComments;
using RepositoryContracts;

namespace CLI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;

    public SinglePostView(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
    }

    public async Task DisplayAsync(Guid postId)
    {
        var post = await postRepository.GetSingleAsync(postId);
        if (post == null)
        {
            Console.WriteLine("Post not found.");
            return;
        }

        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Content: {post.Content}");
        Console.WriteLine($"User ID: {post.UserId}");
        Console.WriteLine($"Created At: {post.CreatedAt}");

        var manageCommentsView = new ManageCommentsView(commentRepository, userRepository, postId);
        await manageCommentsView.DisplayAsync();
    }
}
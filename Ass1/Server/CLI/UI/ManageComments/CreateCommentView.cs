using Entities;
using RepositoryContracts;

namespace CLI.ManageComments;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly Guid postId;

    public CreateCommentView(ICommentRepository commentRepository, IUserRepository userRepository, Guid postId)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.postId = postId;
    }

    public async Task DisplayAsync()
    {
        Guid userId = Guid.NewGuid(); // Temporary
        Console.WriteLine("Enter comment content:");
        string content = Console.ReadLine();

        var comment = new Comment
        {
            CommentId = Guid.NewGuid(),
            UserId = userId,
            PostId = postId,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };

        await commentRepository.AddAsync(comment);
        Console.WriteLine("Comment added successfully.");
    }
}
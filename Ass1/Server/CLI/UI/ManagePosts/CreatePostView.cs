using Entities;
using RepositoryContracts;

namespace CLI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreatePostView(IPostRepository postRepository, IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task DisplayAsync()
    {
        Console.WriteLine("Enter post title:");
        string title = Console.ReadLine();

        Console.WriteLine("Enter post content:");
        string content = Console.ReadLine();

        Console.WriteLine("Enter user ID for the post:");
        string userIdInput = Console.ReadLine();
        Guid userId;
        if (!Guid.TryParse(userIdInput, out userId))
        {
            Console.WriteLine("Invalid user ID.");
            return;
        }

        var post = new Post
        {
            PostId = Guid.NewGuid(),
            Title = title,
            Content = content,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await postRepository.AddAsync(post);
        Console.WriteLine("Post created successfully.");
    }
}
using CLI.ManageComments;
using CLI.ManageVotes;
using Entities;
using RepositoryContracts;
using CLI.ManageVotes;


namespace CLI.ManagePosts;

public class ManageSinglePost
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly IVoteRepository voteRepository;
    private readonly Guid postId;

    public ManageSinglePost(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository, IVoteRepository voteRepository, Guid postId)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.voteRepository = voteRepository;
        this.postId = postId;
    }

    public async Task DisplayAsync()
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

        bool running = true;
        while (running)
        {
            Console.WriteLine("1. Comments");
            Console.WriteLine("2. Vote");
            Console.WriteLine("3. Back to Post List");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    var manageCommentsView = new ManageCommentsView(commentRepository, userRepository, postId);
                    await manageCommentsView.DisplayAsync();
                    break;
                case "2":
                    var manageVotes = new ManageVotes.ManageVotes(voteRepository, userRepository, postId, VoteType.Post); // Pass the current user's ID
                    await manageVotes.DisplayAsync();
                    break;

                case "3":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}

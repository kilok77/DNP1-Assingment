using RepositoryContracts;

namespace CLI.ManagePosts;

public class ManagePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IVoteRepository voteRepository;

    public ManagePostView(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository, IVoteRepository voteRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.voteRepository = voteRepository;
    }

    public async Task DisplayAsync()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("Manage Posts:");
            Console.WriteLine("1. Create Post");
            Console.WriteLine("2. List All Posts");
            Console.WriteLine("3. Back to Main Menu");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    var createPostView = new CreatePostView(postRepository, userRepository);
                    await createPostView.DisplayAsync();
                    break;
                case "2":
                    var listPostView = new ListPostView(postRepository, userRepository, commentRepository, voteRepository);
                    listPostView.Display();
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

using CLI.ManagePosts;
using CLI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IVoteRepository voteRepository;

    public CliApp(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository, IVoteRepository voteRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.voteRepository = voteRepository;
    }

    public async Task StartAsync()
    {
        Console.WriteLine("Welcome to the CLI App!");

        bool running = true;
        while (running)
        {
            Console.WriteLine("Choose an action: ");
            Console.WriteLine("1. Manage Users");
            Console.WriteLine("2. Manage Posts");
            Console.WriteLine("3. Exit");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    var manageUsersView = new ManageUsersView(userRepository);
                    await manageUsersView.DisplayAsync();
                    break;
                case "2":
                    var managePostView = new ManagePostView(postRepository, userRepository, commentRepository, voteRepository);
                    await managePostView.DisplayAsync();
                    break;
                case "3":
                    running = false;
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}
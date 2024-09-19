using RepositoryContracts;

namespace CLI.ManageComments;

public class ManageCommentsView
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly Guid postId;

    public ManageCommentsView(ICommentRepository commentRepository, IUserRepository userRepository, Guid postId)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.postId = postId;
    }

    public async Task DisplayAsync()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("Manage Comments:");
            Console.WriteLine("1. View Comments");
            Console.WriteLine("2. Add Comment");
            Console.WriteLine("3. Back to Post View");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    var listCommentsView = new ListCommentsView(commentRepository, postId);
                    listCommentsView.Display();
                    break;
                case "2":
                    var createCommentView = new CreateCommentView(commentRepository, userRepository, postId);
                    await createCommentView.DisplayAsync();
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
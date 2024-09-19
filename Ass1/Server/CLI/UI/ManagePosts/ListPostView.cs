using RepositoryContracts;

namespace CLI.ManagePosts;

public class ListPostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IVoteRepository voteRepository;


    public ListPostView(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository, IVoteRepository voteRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.voteRepository = voteRepository;;
    }

    public void Display()
    {
        const int pageSize = 10;
        var posts = postRepository.GetManyAsync().ToList();
        int totalPosts = posts.Count;
        int currentPage = 0;

        while (true)
        {
            int skip = currentPage * pageSize;
            var postsToShow = posts.Skip(skip).Take(pageSize).ToList();

            if (!postsToShow.Any())
            {
                Console.WriteLine("No more posts to show.");
                return;
            }

            Console.WriteLine("Select a Post by Number:");
            for (int i = 0; i < postsToShow.Count; i++)
            {
                var post = postsToShow[i];
                Console.WriteLine($"{i + 1}. {post.Title} (ID: {post.PostId})");
            }

            Console.WriteLine($"Showing page {currentPage + 1} of {(totalPosts + pageSize - 1) / pageSize}");
            Console.WriteLine("Enter a number to select a post, 'N' for next page, or 'Q' to quit:");

            string input = Console.ReadLine();

            if (input.Equals("N", StringComparison.OrdinalIgnoreCase))
            {
                currentPage++;
                continue;
            }
            else if (input.Equals("Q", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (int.TryParse(input, out int selectedNumber) && selectedNumber > 0 && selectedNumber <= postsToShow.Count)
            {
                var selectedPost = postsToShow[selectedNumber - 1];
                var manageSinglePost = new ManageSinglePost(postRepository,commentRepository,userRepository, voteRepository, selectedPost.PostId);
                manageSinglePost.DisplayAsync().Wait();
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
        }
    }
}

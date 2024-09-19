// See https://aka.ms/new-console-template for more information

using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app ...");
IUserRepository userRepository = new UserInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();
IVoteRepository voteRepository = new VoteInMemoryRepository();

CliApp cliApp = new CliApp(postRepository, userRepository, commentRepository, voteRepository);
await cliApp.StartAsync();
// See https://aka.ms/new-console-template for more information

using CLI.UI;
using FileRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app ...");
IUserRepository userRepository = new UserFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();
IPostRepository postRepository = new PostFileRepository();
IVoteRepository voteRepository = new VoteFileRepository();

CliApp cliApp = new CliApp(postRepository, userRepository, commentRepository, voteRepository);
await cliApp.StartAsync();
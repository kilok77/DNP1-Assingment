using RepositoryContracts;

namespace CLI.ManageUsers;

public class ManageUsersView
{
    private readonly IUserRepository userRepository;

    public ManageUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task DisplayAsync()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("Manage Users:");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. List All Users");
            Console.WriteLine("3. Back to Main Menu");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    var createUserView = new CreateUserView(userRepository);
                    await createUserView.DisplayAsync();
                    break;
                case "2":
                    var listUsersView = new ListUsersView(userRepository);
                    listUsersView.Display();
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
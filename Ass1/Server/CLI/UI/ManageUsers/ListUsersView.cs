using RepositoryContracts;

namespace CLI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public void Display()
    {
        var users = userRepository.GetMany();

        if (!users.Any())
        {
            Console.WriteLine("No users available.");
        }
        else
        {
            Console.WriteLine("List of users:");
            foreach (var user in users)
            {
                Console.WriteLine($"User ID: {user.Id}, Username: {user.Username}, Email: {user.Email}");
            }
        }
    }
}
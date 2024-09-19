using Entities;
using RepositoryContracts;

namespace CLI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task DisplayAsync()
    {
        Console.WriteLine("Enter username:");
        string username = Console.ReadLine();

        Console.WriteLine("Enter email:");
        string email = Console.ReadLine();

        var user = new User
        {
            UserId = Guid.NewGuid(),
            UserName = username,
            Email = email
        };

        await userRepository.AddAsync(user);
        Console.WriteLine("User created successfully.");
    }
}
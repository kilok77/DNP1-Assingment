// using Server
namespace Entities;

public class User
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public User()
    {
        
    }
    public User(string userName, string email, string password)
    {
        UserName = userName;
        Email = email;
        Password = password;
        UserId = Guid.NewGuid();
    }
}

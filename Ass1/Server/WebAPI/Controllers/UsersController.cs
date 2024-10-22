using ApiContracts;
using ApiContracts.UserDto;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody] CreateUserDto request)
    {
        // Uncomment if you implement username availability verification
        // await VerifyUserNameIsAvailableAsync(request.UserName);

        User user = new User
        {
            UserId = Guid.NewGuid(), // Ensure a new GUID is generated for each user
            UserName = request.UserName,
            Email = request.Email
        };

        User created = await userRepository.AddAsync(user);
        UserDto dto = new()
        {
            Id = created.UserId,
            Username = created.UserName,
            Email = created.Email
        };
        return Created($"users/{dto.Id}", dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUserAsync(Guid id, [FromBody] UserDto request)
    {
        var user = new User
        {
            UserId = id,
            UserName = request.Username,
            Email = request.Email
        };

        await userRepository.UpdateAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(Guid id)
    {
        await userRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserAsync(Guid id)
    {
        var user = await userRepository.GetSingleAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        UserDto dto = new()
        {
            Id = user.UserId,
            Username = user.UserName,
            Email = user.Email
        };
        return Ok(dto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetAllUsers()
    {
        var users = userRepository.GetMany().ToList(); // ToList to materialize the IQueryable
        var userDtos = users.Select(u => new UserDto
        {
            Id = u.UserId,
            Username = u.UserName,
            Email = u.Email
        });

        return Ok(userDtos);
    }

    private async Task VerifyUserNameIsAvailableAsync(string username)
    {
        // Logic to verify if the username is already taken.
        // This might involve checking against the userRepository.
    }
}

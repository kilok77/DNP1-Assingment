using ApiContracts;
using ApiContracts.UserDto;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using RepositoryContracts;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher<User> passwordHasher;

        public UsersController(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUserAsync([FromBody] CreateUserDto request)
        {
            // Validate that email or username are unique (if needed)
            // await VerifyEmailIsAvailableAsynzc(request.Email);

            // Create the user entity
            User user = new User
            {
                Id = Guid.NewGuid(), // Ensure a new GUID is generated for each user
                Username = request.Username,
                Email = request.Email
            };

            // Hash the password and assign it to the user
            var hashedPassword = passwordHasher.HashPassword(user, request.Password);
            user.Password = hashedPassword; // Store the hashed password in the User entity

            // Save the user to the repository
            User created = await userRepository.AddAsync(user);

            // Convert to UserDto to return to the client
            UserDto dto = new UserDto
            {
                Id = created.Id,
                Username = created.Username,
                Email = created.Email
            };

            // Return the created user
            return Created($"users/{dto.Id}", dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUserAsync(Guid id, [FromBody] UserDto request)
        {
            var user = new User
            {
                Id = id,
                Username = request.Username,
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

            UserDto dto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
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
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            });

            return Ok(userDtos);
        }

        private async Task VerifyEmailIsAvailableAsync(string email)
        {
            // Check if a user already exists with the given email
            var existingUser = await userRepository.GetSingleAsync(email);
    
            if (existingUser != null)
            {
                // If a user with this email already exists, throw a conflict error
                throw new Exception("Email is already in use.");
            }
        }

    }
}

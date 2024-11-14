using ApiContracts;
using ApiContracts.UserDto;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher; // Inject the password hasher

        public AuthController(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher; // Initialize the password hasher
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Step 1: Find user by email
                var user = await _userRepository.GetSingleAsync(loginDto.Email);

                // Step 2: If user does not exist, return Unauthorized
                if (user == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                // Step 3: Use PasswordHasher to verify the provided password
                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);

                if (passwordVerificationResult == PasswordVerificationResult.Failed)
                {
                    return Unauthorized("Invalid email or password.");
                }

                // Step 4: Convert the User to UserDto
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                };

                // Step 5: Return the UserDto
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors (e.g., log error)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}

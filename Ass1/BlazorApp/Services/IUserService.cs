using ApiContracts.UserDto;

namespace BlazorApp.Services;

public interface IUserService 
{ 
    public Task<UserDto> AddUserAsync(CreateUserDto request); 
    public Task<UserDto> GetUserAsync(Guid userId); 
    
    public Task<List<UserDto>> GetUsersAsync();
    // public Task UpdateUserAsync(int id, UpdateUserDto request); 
// ... more methods
}
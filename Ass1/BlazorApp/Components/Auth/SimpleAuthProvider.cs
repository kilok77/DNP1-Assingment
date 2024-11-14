
using System.Security.Claims;
using System.Text.Json;
using ApiContracts.UserDto;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity.Data;

namespace BlazorApp.Components.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private ClaimsPrincipal currentClaimsPrincipal;


    public SimpleAuthProvider(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task Login(string email, string password)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "auth/login",
            new LoginRequest { Email = email, Password = password });

        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        UserDto userDto = JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userDto.Username),
            new Claim("Id", userDto.Id.ToString()),
            // Add more claims here with your own claim type as a string, e.g.:
            // new Claim("DateOfBirth", userDto.DateOfBirth.ToString("yyyy-MM-dd"))
            // new Claim("IsAdmin", userDto.IsAdmin.ToString())
            // new Claim("IsModerator", userDto.IsModerator.ToString())
            // new Claim("Email", userDto.Email)
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");

        currentClaimsPrincipal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(currentClaimsPrincipal))
        );
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Return the current authentication state with the currentClaimsPrincipal
        return new AuthenticationState(currentClaimsPrincipal ?? new ());
    }

    public void Logout()
    {
        currentClaimsPrincipal = new(); 
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));
    }
}

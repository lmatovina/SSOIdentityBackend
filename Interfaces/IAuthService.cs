// Interfaces/IAuthService.cs
using System.Threading.Tasks;
using SSO.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

public interface IAuthService
{
    Task<IdentityResult> RegisterAsync(RegisterDto model); 
    Task<string> LoginAsync(LoginDto model);
}
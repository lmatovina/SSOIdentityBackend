using System.Threading.Tasks;
using SSO.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersWithRolesAsync();
}
using SSO.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsersWithRoles()
    {
        var users = await _userService.GetAllUsersWithRolesAsync();
        return Ok(users);
    }
}
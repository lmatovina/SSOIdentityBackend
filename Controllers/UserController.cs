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

    [HttpPut("update-role")]
    public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleDto dto)
    {
        if (dto == null || string.IsNullOrEmpty(dto.userId) || string.IsNullOrEmpty(dto.newRole))
        {
            return BadRequest("Invalid user role update request.");
        }
        try
        {
            await _userService.UpdateUserRoleAsync(dto);
            return Ok(new { message = "User role updated successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SSO.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Servis za autentifikaciju korisnika.
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }
    /// <summary>
    /// Registracija novog korisnika.
    /// </summary>
    /// <param name="model">Sadrzi podatke o korisniku</param>
    /// <returns></returns>
    public async Task<IdentityResult> RegisterAsync(RegisterDto model)
    {
        var user = new ApplicationUser
        {
            UserName = model.email,
            Email = model.email,
            Ime = model.ime,
            Prezime = model.prezime
        };

        var result = await _userManager.CreateAsync(user, model.password);

        if (result.Succeeded)
        {
            // Dodaj rolu "User" nakon uspješne registracije
            await _userManager.AddToRoleAsync(user, "User");
        }

        return result;
    }
    /// <summary>
    /// Prijava korisnika i generiranje JWT tokena.
    /// </summary>
    /// <param name="model">Sadrzi login podatke korisnika</param>
    /// <returns></returns>
    public async Task<string> LoginAsync(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.email);
        if (user == null) return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.password, false);
        if (!result.Succeeded) return null;

        // Create JWT token or return some token string here
        var token = await GenerateJwtToken(user);

        return token;
    }
    /// <summary>
    /// Generira JWT token za korisnika.
    /// </summary>
    /// <param name="user">Korisnicki podaci za generiranje tokena</param>
    /// <returns>JWT token sa korisnickim podacima</returns>
    /// <exception cref="Exception"></exception>
    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var keyString = _configuration["Jwt:Key"];

        if (string.IsNullOrEmpty(keyString))
            throw new Exception("JWT Key is not configured. Provjeri appsettings.json i naziv ključa.");

        // Inicijalne claimove
        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
    };

        // Dohvati role korisnika iz Identity baze
        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Kreiranje tokena
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(60);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

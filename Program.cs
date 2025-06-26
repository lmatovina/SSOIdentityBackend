using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSO.IdentityServer.Data;
using SSO.IdentityServer.Models;
using SSO.IdentityServer.Config;
using Duende.IdentityServer;
using Duende.IdentityServer.AspNetIdentity;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       "Server=DESKTOP-ECS032T\\SQLEXPRESS;Database=SSOIdentityDb;Trusted_Connection=True;TrustServerCertificate=True";

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();  // Dodano ako koristiš cookie ili auth header
                      });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer(options =>
{
    options.EmitStaticAudienceClaim = true;
})
    .AddInMemoryClients(ClientConfig.Clients)
    .AddInMemoryApiScopes(ScopeConfig.ApiScopes)
    .AddInMemoryIdentityResources(IdentityResourceConfig.IdentityResources)
    .AddAspNetIdentity<ApplicationUser>()
    .AddDeveloperSigningCredential();

builder.Services.AddAuthentication()
    .AddCookie();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

// Dodaj mapiranje kontrolera (važno!)
builder.Services.AddControllers();

var app = builder.Build();

await SeedDataAsync(app);

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

// Ovo mapira sve kontrolere s atributima [ApiController] i rutama
app.MapControllers();

app.MapGet("/", () => "IdentityServer je ziv");

app.Run();

static async Task SeedDataAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await ApplicationDbContextSeed.SeedRolesAndAdminAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Greška prilikom seeding podataka: {Message}", ex.Message);
        throw;
    }
}

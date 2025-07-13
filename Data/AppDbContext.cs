using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSO.IdentityServer.Models;


namespace SSO.IdentityServer.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
	public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options)
	{
}

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Document> Documents { get; set; }
}

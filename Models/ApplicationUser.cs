using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SSO.IdentityServer.Models;

public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Ime korisnika.
    /// </summary>
    [Required(ErrorMessage = "Ime je obavezno")]
    [Display(Name = "Ime")]
    [StringLength(50)]
    public string Ime { get; set; } = null!;

    /// <summary>
    /// Prezime korisnika.
    /// </summary>
    [Required(ErrorMessage = "Prezime je obavezno")]
    [Display(Name = "Prezime")]
    [StringLength(50)]
    public string Prezime { get; set; } = null!;
}

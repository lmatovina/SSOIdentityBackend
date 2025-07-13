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
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SSO.IdentityServer.Data;
using System.IO;


public class DocumentService : IDocumentService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public DocumentService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }

    public async Task UploadDocumentAsync(UploadDocumentDto dto)
    {
        using var ms = new MemoryStream();          // <-- Ovdje deklariraš ms
        await dto.Data.CopyToAsync(ms);              // Kopiraš file stream u memorijski stream

        var document = new Document
        {
            FileName = dto.Data.FileName,
            ContentType = dto.Data.ContentType,
            Data = ms.ToArray(),                      // Pretvara memorijski stream u byte array
            UploadedByUserId = dto.UserId,
            UploadedAt = DateTime.UtcNow
        };

        await _context.Documents.AddAsync(document);
        await _context.SaveChangesAsync();
    }
}

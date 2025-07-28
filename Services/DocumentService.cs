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
using System.Diagnostics.Eventing.Reader;


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
        using var ms = new MemoryStream();         
        await dto.Data.CopyToAsync(ms);              

        var document = new Document
        {
            FileName = dto.Data.FileName,
            ContentType = dto.Data.ContentType,
            Data = ms.ToArray(),                      
            UploadedByUserId = dto.UserId,
            UploadedAt = DateTime.UtcNow
        };

        await _context.Documents.AddAsync(document);
        await _context.SaveChangesAsync();
    }

    public async Task<List<DocumentDto>> GetDocumentsByUserId(string userId)
    {
        var documents = await _context.Documents
            .Where(d => d.UploadedByUserId == userId)
            .Include(d => d.UploadedByUser)
            .ToListAsync();


        var documentDtos = documents.Select(d => new DocumentDto
        {
            Id = d.Id,
            FileName = d.FileName,
            ContentType = d.ContentType,
            UploadedByUserId = d.UploadedByUserId,
            UploadedAt = d.UploadedAt
        }).ToList();

        return documentDtos;
    }


}

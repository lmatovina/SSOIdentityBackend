using System.Threading.Tasks;
using SSO.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

public interface IDocumentService
{
    Task UploadDocumentAsync(UploadDocumentDto dto);
}
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace SSO.IdentityServer.Models;

public class Document
{
    public int Id { get; set; }

    public string FileName { get; set; }

    public string ContentType { get; set; }

    public byte[] Data { get; set; }

    public string? UploadedByUserId { get; set; }

    public ApplicationUser? UploadedByUser { get; set; }

    public DateTime UploadedAt { get; set; }
}
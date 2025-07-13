using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

    public class RegisterDto
    {
        public string email { get; set; }
        public string password { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
    }

    public class LoginDto
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }

    public class UpdateUserRoleDto
    {
        [Required]
        [JsonPropertyName("userId")]
        public string userId { get; set; }

        [Required]
        [JsonPropertyName("newRole")]
        public string newRole { get; set; }
    }

    public class UploadDocumentDto
    {
    public IFormFile Data { get; set; } // prima file iz forme
    public string UserId { get; set; } = string.Empty;
    }

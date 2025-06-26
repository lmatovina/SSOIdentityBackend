using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
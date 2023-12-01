using System.ComponentModel.DataAnnotations;

namespace UserManagement.WebApi.Models;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
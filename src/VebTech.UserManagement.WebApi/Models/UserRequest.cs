namespace VebTech.UserManagement.WebApi.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Email), IsUnique = true)]
public class UserRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    [Range(0, 100)]
    public int Age { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
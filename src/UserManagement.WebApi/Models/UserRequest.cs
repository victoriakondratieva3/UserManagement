using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.WebApi.Models;

[Index(nameof(Email), IsUnique = true)]
public class UserRequest
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [Range(0, 100)]
    public int Age { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
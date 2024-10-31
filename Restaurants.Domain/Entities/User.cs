using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Restaurants.Domain.Entities;

public class User : IdentityUser
{
    public DateOnly? DateOfBirth { get; set; }

    [MaxLength(50)] public string? Nationality { get; set; }
}
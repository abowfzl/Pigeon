using Microsoft.AspNetCore.Identity;

namespace Pigeon.Models.DTOs;

public class UserDto
{
    public string? Name { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Password { get; set; }

    public static implicit operator UserDto(IdentityUser user)
    {
        return new()
        {
            Name = user.UserName,
            PhoneNumber = user.PhoneNumber,
        };
    }
}
using Demo.Domain.Models;

namespace Demo.Api.Dtos;

public class UserDto
{
    public string Username { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; } = default;

    public UserDto()
    {
    }

    public UserDto(User user) {
        Username = user.Username;
        Name = user.Name;
        DateOfBirth = user.DateOfBirth;
    }
}

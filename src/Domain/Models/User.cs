namespace Demo.Domain.Models;

public class User
{
    public int Id { get; set; } = default;

    public string Username { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; } = default;
}

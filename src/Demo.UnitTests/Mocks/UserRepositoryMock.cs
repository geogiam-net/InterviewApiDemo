using Demo.Domain.Exceptions;
using Demo.Domain.Interfaces;
using Demo.Domain.Models;
using Demo.Domain.Validators;

namespace Demo.UnitTests.Mocks;

public class UserRepositoryMock(IMessageBroker messageBroker, TimeProvider timeProvider) : IUserRepository
{
    private static readonly List<User> UserList = new();

    public async Task AddUserAsync(string username, string name, DateOnly dateOfBirth,
        CancellationToken cancellationToken = default)
    {
        var newUser = new User
        {
            Username = username,
            Name = name,
            DateOfBirth = dateOfBirth
        };

        var errors = UserValidator.Validate(newUser, timeProvider.GetUtcNow().UtcDateTime);
        if (errors.Length > 0)
        {
            throw new ValidationException(errors);
        }

        var existingUser = await GetUserAsync(username);
        if (existingUser is not null)
        {
            throw new ValidationException(["User already exists"]);
        }

        UserList.Add(newUser);

        await messageBroker.SendUserCreatedAsync(newUser);
    }

    public async Task<User?> GetUserAsync(string username, CancellationToken cancellationToken = default)
    {
        return UserList.FirstOrDefault(x => x.Username == username);
    }
}
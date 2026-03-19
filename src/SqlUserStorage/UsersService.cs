using Demo.Domain.Exceptions;
using Demo.Domain.Interfaces;
using Demo.Domain.Models;
using Demo.Domain.Validators;
using Demo.Infrastructure.SqlStorage.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure.SqlStorage;

public class UsersService(ApplicationDbContext dbContext, IMessageBroker messageBroker, TimeProvider timeProvider) : IUserRepository
{
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
        if(errors.Length > 0)
        {
            throw new DomainException(errors);
        }

        var existingUser = await GetUserAsync(username);
        if (existingUser is not null)
        {
            throw new DomainException(["User already exists"]);
        }

        dbContext.Users.Add(newUser);
        await dbContext.SaveChangesAsync(cancellationToken);

        await messageBroker.SendUserCreatedAsync(newUser);
    }

    public async Task<User?> GetUserAsync(string username, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    }
}
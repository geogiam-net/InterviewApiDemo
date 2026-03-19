using Demo.Domain.Models;

namespace Demo.Domain.Interfaces
{
    public interface IUserRepository
    {
        public Task AddUserAsync(string username, string name, DateOnly dateOfBirth,
            CancellationToken cancellationToken = default);

        public Task<User?> GetUserAsync(string username, CancellationToken cancellationToken = default);
    }
}
using InterviewApiDemo.Data;
using Microsoft.EntityFrameworkCore;
using InterviewApiDemo.Models;

namespace InterviewApiDemo.Services;

public class UsersService(ApplicationDbContext dbContext)
{
    public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetUserAsync(string username, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    }

}

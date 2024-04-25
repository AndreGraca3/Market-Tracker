using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.User;

using User = Domain.User;

public class UserRepository(
    MarketTrackerDataContext dataContext
) : IUserRepository
{
    public async Task<IEnumerable<User>> GetUsersAsync(string? username, int skip, int limit)
    {
        return await dataContext.User.Where(user => username == null || user.Username.Contains(username)).Skip(skip)
            .Take(limit).Select(userEntity => userEntity.ToUser()).ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return (await dataContext.User.FindAsync(id))?.ToUser();
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return (await dataContext.User.FirstOrDefaultAsync(user => user.Username == username))?.ToUser();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return (await dataContext.User.FirstOrDefaultAsync(user => user.Email == email))?.ToUser();
    }

    public async Task<Guid> CreateUserAsync(string username, string name, string email, string role)
    {
        var newUser = new UserEntity
        {
            Name = name,
            Username = username,
            Email = email,
            Role = role
        };
        await dataContext.User.AddAsync(newUser);
        await dataContext.SaveChangesAsync();
        return newUser.Id;
    }

    public async Task<User?> UpdateUserAsync(Guid id, string? name, string? userName)
    {
        var userEntity = await dataContext.User.FindAsync(id);
        if (userEntity is null)
        {
            return null;
        }

        userEntity.Name = name ?? userEntity.Name;
        userEntity.Username = userName ?? userEntity.Username;

        await dataContext.SaveChangesAsync();
        return userEntity.ToUser();
    }

    public async Task<User?> DeleteUserAsync(Guid id)
    {
        var deletedUserEntity = await dataContext.User.FindAsync(id);
        if (deletedUserEntity is null)
        {
            return null;
        }

        dataContext.Remove(deletedUserEntity);
        await dataContext.SaveChangesAsync();
        return deletedUserEntity.ToUser();
    }
}
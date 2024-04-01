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
        var users = username is null
            ? await dataContext.User.Skip(skip).Take(limit).ToListAsync()
            : await dataContext.User.Where(user => user.Username.Contains(username)).Skip(skip).Take(limit)
                .ToListAsync();

        return users.Select(MapUserEntity)!;
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return MapUserEntity(await dataContext.User.FindAsync(id));
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return MapUserEntity(await dataContext.User.FirstOrDefaultAsync(user => user.Username == username));
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return MapUserEntity(await dataContext.User.FirstOrDefaultAsync(user => user.Email == email));
    }

    public async Task<Guid> CreateUserAsync(string username, string name, string email, string password)
    {
        var newUser = new UserEntity
        {
            Name = name,
            Username = username,
            Email = email,
            Password = password
        };
        await dataContext.User.AddAsync(newUser);
        await dataContext.SaveChangesAsync();
        return newUser.Id;
    }

    public async Task<User?> UpdateUserAsync(Guid id, string? name, string? userName)
    {
        var user = await dataContext.User.FindAsync(id);
        if (user is null)
        {
            return null;
        }

        user.Name = name ?? user.Name;
        user.Username = userName ?? user.Username;

        await dataContext.SaveChangesAsync();
        return MapUserEntity(user);
    }

    public async Task<User?> DeleteUserAsync(Guid id)
    {
        var deletedUser = await dataContext.User.FindAsync(id);
        if (deletedUser is null)
        {
            return null;
        }

        dataContext.Remove(deletedUser);
        await dataContext.SaveChangesAsync();
        return MapUserEntity(deletedUser);
    }

    private static User? MapUserEntity(UserEntity? userEntity)
    {
        return userEntity is not null
            ? new User
            (
                userEntity.Id,
                userEntity.Username,
                userEntity.Name,
                userEntity.Email,
                userEntity.Password,
                userEntity.CreatedAt)
            : null;
    }
}
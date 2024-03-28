using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.User;

using User = Domain.User;

public class UserRepository(
    MarketTrackerDataContext dataContext
) : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return MapUserEntity(await dataContext.User.FindAsync(id));
    }

    public async Task<Guid> CreateUserAsync(string name, string username, string email, string password)
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

    public async Task<User?> GetUserByNameAsync(string name)
    {
        return MapUserEntity(await dataContext.User.FirstOrDefaultAsync(user => user.Name == name));
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return MapUserEntity(await dataContext.User.FirstOrDefaultAsync(user => user.Email == email));
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
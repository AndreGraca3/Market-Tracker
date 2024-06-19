using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Users;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Account.Users.User;

using User = Domain.Schemas.Account.Users.User;

public class UserRepository(
    MarketTrackerDataContext dataContext
) : IUserRepository
{
    public async Task<PaginatedResult<UserItem>> GetUsersAsync(string? role, int skip, int limit)
    {
        var allUsers = dataContext.User.Where(user =>
            role == null || user.Role.Equals(role));

        var users = await allUsers
            .Skip(skip)
            .Take(limit).Select(userEntity => userEntity.ToUserItem())
            .ToListAsync();

        return new PaginatedResult<UserItem>(users, allUsers.Count(), skip, limit);
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return (await dataContext.User.FindAsync(id))?.ToUser();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return (await dataContext.User.FirstOrDefaultAsync(user => user.Email == email))?.ToUser();
    }
    
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return (await dataContext.User.FirstOrDefaultAsync(user => user.Name == username))?.ToUser();
    }

    public async Task<UserId> CreateUserAsync(string name, string email, string role)
    {
        var newUser = new UserEntity
        {
            Name = name,
            Email = email,
            Role = role,
            CreatedAt = DateTime.Now
        };
        await dataContext.User.AddAsync(newUser);
        await dataContext.SaveChangesAsync();
        return new UserId(newUser.Id);
    }

    public async Task<User?> UpdateUserAsync(Guid id, string? name)
    {
        var userEntity = await dataContext.User.FindAsync(id);
        if (userEntity is null)
        {
            return null;
        }

        userEntity.Name = name ?? userEntity.Name;

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
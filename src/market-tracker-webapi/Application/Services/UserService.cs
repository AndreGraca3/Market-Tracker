using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Services
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly MarketTrackerDataContext _marketTrackerDataContext;

        public UserService(ILogger<UserService> logger, MarketTrackerDataContext marketTrackerDataContext)
        {
            _logger = logger;
            _marketTrackerDataContext = marketTrackerDataContext;
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _marketTrackerDataContext.User.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByName(string name)
        {
            return await _marketTrackerDataContext.User.FirstOrDefaultAsync(u => u.Name == name);
        }

        public async Task<User?> CreateUser(User user)
        {
            return await TransactionHandler(async () =>
            {
                var newUser = new UserEntity
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Avatar = user.Avatar
                };

                _marketTrackerDataContext.User.Add(newUser);
                await _marketTrackerDataContext.SaveChangesAsync();

                return newUser;
            });
        }

        public async Task<User?> UpdateUser(User user)
        {
            return await TransactionHandler(async () =>
            {
                var existingUser = await _marketTrackerDataContext.User.FirstOrDefaultAsync(u => u.Id == user.Id);

                if (existingUser == null)
                {
                    return null;
                }

                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.Avatar = user.Avatar;

                await _marketTrackerDataContext.SaveChangesAsync();

                return existingUser;
            });
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await TransactionHandler(async () =>
            {
                var user = await _marketTrackerDataContext.User.FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return false;
                }

                _marketTrackerDataContext.User.Remove(user);
                await _marketTrackerDataContext.SaveChangesAsync();

                return true;
            });
        }
    }
}

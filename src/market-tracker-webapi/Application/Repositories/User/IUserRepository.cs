using market_tracker_webapi.Application.Models;

namespace market_tracker_webapi.Application.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(int id);
    }
}

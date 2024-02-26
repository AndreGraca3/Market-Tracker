using market_tracker_webapi.Application.Models;

namespace market_tracker_webapi.Application.Queries
{
    public interface IUserQuery
    {
        Task<User?> GetUser(int id);
    }
}

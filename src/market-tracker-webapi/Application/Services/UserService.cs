using market_tracker_webapi.Application.Repository;
using market_tracker_webapi.Infrastructure;

namespace market_tracker_webapi.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
    }
}

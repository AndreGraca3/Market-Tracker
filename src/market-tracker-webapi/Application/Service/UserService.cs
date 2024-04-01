using market_tracker_webapi.Application.Repository.Operations.User;

namespace market_tracker_webapi.Application.Service
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

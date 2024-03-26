using market_tracker_webapi.Application.Repositories.User;

namespace market_tracker_webapi.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly TransactionManager.TransactionManager _transactionManager;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, TransactionManager.TransactionManager transactionManager,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _transactionManager = transactionManager;
            _logger = logger;
        }
        
        //public async Either<> CreateUser()
        //{
        //    
        //}
    }
}
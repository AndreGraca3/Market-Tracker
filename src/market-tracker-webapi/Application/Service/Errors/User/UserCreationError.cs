namespace market_tracker_webapi.Application.Service.Errors.User;

public class UserCreationError : IUserError
{
    public class InvalidEmail(string email) : UserCreationError
    {
        public string Email { get; } = email;
    }

    public class EmailAlreadyInUse(string email) : UserCreationError
    {
        public string Email { get; } = email;
    }
}
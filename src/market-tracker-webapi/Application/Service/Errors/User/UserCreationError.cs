namespace market_tracker_webapi.Application.Service.Errors.User;

public class UserCreationError : IUserError
{
    public class InvalidEmail(string email) : UserCreationError
    {
        public string Email { get; } = email;
    }

    public class CredentialAlreadyInUse(string credentialValue, string credentialName) : UserCreationError
    {
        public string CredentialName { get; } = credentialName;
        public string CredentialValue { get; } = credentialValue;
    }
    
    public class InvalidUsername(string username) : UserCreationError
    {
        public string Username { get; } = username;
    }
}
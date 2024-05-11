namespace market_tracker_webapi.Application.Service.Errors.PreRegister;

public class PreRegistrationCreationError: IPreRegistrationError
{
 
    public class EmailAlreadyInUse(string email) : PreRegistrationCreationError
    {
        public string Email { get; } = email;
    }
}
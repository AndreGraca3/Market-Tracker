namespace market_tracker_webapi.Application.Service.Errors.PreRegister;

public class PreRegistrationFetchingError : IPreRegistrationError
{
    public class PreRegistrationByIdNotFound(Guid id) : PreRegistrationFetchingError
    {
        public Guid Id { get; } = id;
    }

    public class PreRegistrationInvalidData(string message) : PreRegistrationFetchingError
    {
        public string Message { get; } = message;
    }

    public class PreRegistrationNotValidated(Guid code): PreRegistrationFetchingError
    {
        public Guid Code { get; } = code;
    }
}
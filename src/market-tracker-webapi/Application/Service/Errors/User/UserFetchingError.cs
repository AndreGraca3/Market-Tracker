namespace market_tracker_webapi.Application.Service.Errors.User;

public class UserFetchingError: UserError
{
    public class UserByIdNotFound(Guid id) : UserFetchingError
    {
        public Guid Id { get; } = id;
    }

    public class UserByTokenValueNotFound(Guid tokenValue) : UserFetchingError
    {
        public Guid TokenValue { get; } = tokenValue;
    }
}
namespace market_tracker_webapi.Application.Service.Errors.User;

public class UserFetchingError: IUserError
{
    public class UserByIdNotFound(Guid id) : UserFetchingError
    {
        public Guid Id { get; } = id;
    }
    
    public class DeviceTokenNotFound(Guid clientId, string deviceId) : UserFetchingError
    {
        public Guid ClientId { get; } = clientId;
        public string DeviceId { get; } = deviceId;
    }
}
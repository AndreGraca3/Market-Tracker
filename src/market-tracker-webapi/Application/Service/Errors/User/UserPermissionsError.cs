namespace market_tracker_webapi.Application.Service.Errors.User;

public class UserPermissionsError : IUserError
{
    public class UserDoNotOwnList(Guid clientId, int listId) : UserPermissionsError
    {
        public Guid ClientId { get; } = clientId;
        public int ListId { get; } = listId;
    }
}
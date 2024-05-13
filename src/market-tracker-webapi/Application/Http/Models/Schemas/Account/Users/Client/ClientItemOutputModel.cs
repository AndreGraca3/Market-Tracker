using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Client;

public record ClientItemOutputModel(Guid Id, string Username, string? Avatar);

public static class ClientItemOutputModelMapper
{
    public static ClientItemOutputModel ToOutputModel(this ClientItem clientItem) =>
        new ClientItemOutputModel(clientItem.Id.Value, clientItem.Username, clientItem.Avatar);
}
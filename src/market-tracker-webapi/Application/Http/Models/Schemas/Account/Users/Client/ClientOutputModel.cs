namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Client;

using Client = Domain.Schemas.Account.Users.Client;

public record ClientOutputModel(Guid Id, string Username, string Email, DateTime CreatedAt, string? Avatar);

public static class ClientOutputModelMapper
{
    public static ClientOutputModel ToOutputModel(this Client clientItem) =>
        new ClientOutputModel(clientItem.Id.Value, clientItem.Username, clientItem.Email, clientItem.CreatedAt,
            clientItem.AvatarUrl);
}
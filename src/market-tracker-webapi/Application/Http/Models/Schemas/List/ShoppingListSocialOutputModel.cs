using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Client;
using market_tracker_webapi.Application.Service.Results;

namespace market_tracker_webapi.Application.Http.Models.Schemas.List;

public record ShoppingListSocialOutputModel(
    string Id,
    string Name,
    DateTime? ArchivedAt,
    DateTime CreatedAt,
    ClientItemOutputModel Owner,
    IEnumerable<ClientItemOutputModel> Members
);

public static class ShoppingListSocialOutputModelMapper
{
    public static ShoppingListSocialOutputModel ToOutputModel(this ShoppingListResult result)
    {
        return new ShoppingListSocialOutputModel(
            result.Id,
            result.Name,
            result.ArchivedAt,
            result.CreatedAt,
            result.Owner.ToOutputModel(),
            result.Members.Select(m => m.ToOutputModel())
        );
    }
}
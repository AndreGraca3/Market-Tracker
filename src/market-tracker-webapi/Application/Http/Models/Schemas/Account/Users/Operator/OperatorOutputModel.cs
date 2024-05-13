using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Operator;

public record OperatorOutputModel(Guid Id, string Name, int PhoneNumber, string StoreName, DateTime CreatedAt);

public static class OperatorModelMapper
{
    public static OperatorOutputModel ToOutputModel(this OperatorItem @operator)
    {
        return new OperatorOutputModel(
            @operator.Id.Value,
            @operator.Name,
            1, // TODO
            @operator.StoreName,
            DateTime.Now // TODO
        );
    }
}
namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Operator;

public record OperatorOutputModel(Guid Id, string Name, string Email, int PhoneNumber, DateTime CreatedAt);

public record OperatorItemOutputModel(Guid Id, string Name, string CompanyLogoUrl);

public static class OperatorModelMapper
{
    public static OperatorOutputModel ToOutputModel(this Domain.Schemas.Account.Users.Operator @operator)
    {
        return new OperatorOutputModel(
            @operator.Id.Value,
            @operator.Name,
            @operator.Email,
            @operator.PhoneNumber,
            @operator.CreatedAt
        );
    }

    public static OperatorItemOutputModel ToOutputModel(this Domain.Schemas.Account.Users.OperatorItem operatorItem)
    {
        return new OperatorItemOutputModel(
            operatorItem.Id.Value,
            operatorItem.Name,
            operatorItem.CompanyLogoUrl
        );
    }
}
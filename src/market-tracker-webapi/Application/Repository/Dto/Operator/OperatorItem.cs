namespace market_tracker_webapi.Application.Repository.Dto.Operator;

public record OperatorItem(Guid Id, string Name, int PhoneNumber, string StoreName, DateTime CreatedAt);
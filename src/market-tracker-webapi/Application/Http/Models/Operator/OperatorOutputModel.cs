namespace market_tracker_webapi.Application.Http.Models.Operator;

public record OperatorOutputModel(Guid Id, string Name, int PhoneNumber, string StoreName, DateTime CreatedAt);
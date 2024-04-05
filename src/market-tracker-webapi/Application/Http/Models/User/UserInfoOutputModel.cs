namespace market_tracker_webapi.Application.Http.Models.User;

public record UserInfoOutputModel(Guid Id, string Username, string? AvatarUrl);

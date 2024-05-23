using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.User;

public record UserUpdateInputModel([MaxLength(30)] string? Name);
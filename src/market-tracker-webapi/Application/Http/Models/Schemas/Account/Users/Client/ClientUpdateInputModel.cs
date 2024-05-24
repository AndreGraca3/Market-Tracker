using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Client;

public record ClientUpdateInputModel(
    [MaxLength(30)] string? Name,
    [MaxLength(20)] string? Username,
    string? Avatar
);
using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Client;

public record ClientCreationInputModel(
    [Required] [MaxLength(20)] string Username,
    [Required] [MaxLength(30)] string Name,
    [Required] [MaxLength(200)] string Email,
    [Required] [MaxLength(30)] string Password,
    string? Avatar
);
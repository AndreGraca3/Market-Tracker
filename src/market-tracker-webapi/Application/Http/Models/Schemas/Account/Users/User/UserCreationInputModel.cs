using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.User;

public record UserCreationInputModel(
    [Required] [MaxLength(30)] string Name,
    [Required] [MaxLength(200)] string Email,
    [Required] [MaxLength(30)] string Password,
    [Required] string Role
);
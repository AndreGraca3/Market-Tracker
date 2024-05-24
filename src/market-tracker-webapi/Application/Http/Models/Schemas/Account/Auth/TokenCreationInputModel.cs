using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Auth;

public record TokenCreationInputModel(
    [Required] [MaxLength(200)] string Email,
    [Required] [MaxLength(30)] string Password
);
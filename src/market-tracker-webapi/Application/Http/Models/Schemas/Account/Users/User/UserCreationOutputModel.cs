using System.ComponentModel.DataAnnotations;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.User;

public record UserCreationOutputModel(
    Guid Id,
    [RegularExpression(
        "^(Moderator|Client|Operator)$",
        ErrorMessage = "Wrong role provided. Must be 'Moderator', 'Client' or 'Operator'."
    )]
    string Role
);
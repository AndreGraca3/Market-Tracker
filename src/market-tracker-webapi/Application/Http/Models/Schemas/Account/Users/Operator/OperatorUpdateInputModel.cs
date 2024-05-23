using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Operator;

public record OperatorUpdateInputModel(
    [Required]
    [Range(210000000, 999999999)]
    int NewPhoneNumber
);
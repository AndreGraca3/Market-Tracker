using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Operator;

public record OperatorCreationInputModel([Required] [MaxLength(30)] string Password);
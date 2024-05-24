using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Auth;

public record GoogleTokenCreationInputModel([Required] string IdToken);
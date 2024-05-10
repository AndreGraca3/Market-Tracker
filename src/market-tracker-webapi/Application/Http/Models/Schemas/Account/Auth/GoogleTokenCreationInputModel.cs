using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Auth;

public class GoogleTokenCreationInputModel
{
    [Required]
    public string IdToken { get; set; }
}
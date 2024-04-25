using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.GoogleToken;

public class GoogleTokenCreationInputModel
{
    [Required]
    public string IdToken { get; set; }
}
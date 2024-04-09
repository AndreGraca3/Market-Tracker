using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.City;

public class CityCreationInputModel
{
    [Required]
    public string CityName { get; set; }
}

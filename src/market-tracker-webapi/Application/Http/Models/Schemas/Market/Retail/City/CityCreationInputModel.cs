using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.City;

public class CityCreationInputModel
{
    [Required]
    public string CityName { get; set; }
}

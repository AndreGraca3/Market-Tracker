using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.City;

public record CityCreationInputModel([Required] [MaxLength(30)] string CityName);
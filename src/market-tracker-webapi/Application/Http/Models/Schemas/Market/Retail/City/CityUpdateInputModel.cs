using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.City;

public record CityUpdateInputModel([Required] [MaxLength(30)] string CityName);
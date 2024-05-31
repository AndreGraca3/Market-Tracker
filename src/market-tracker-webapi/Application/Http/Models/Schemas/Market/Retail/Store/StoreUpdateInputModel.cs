using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;

public record StoreUpdateInputModel(
    [Required] [MaxLength(30)] string Name,
    [Required] [MaxLength(200)] string Address,
    [Required] int CityId,
    [Required] int CompanyId
);
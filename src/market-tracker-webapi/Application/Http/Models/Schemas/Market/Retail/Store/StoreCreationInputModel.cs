using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;

public record StoreCreationInputModel(
    [Required] [MaxLength(30)] string Name,
    [Required] [MaxLength(200)] string Address,
    int? CityId,
    [Required] int CompanyId,
    [Required] Guid OperatorId
);
using market_tracker_webapi.Application.Domain.Models.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Alert;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Alert;

public interface IAlertService
{
    Task<Either<IServiceError, IEnumerable<PriceAlert>>> GetPriceAlertsByClientIdAsync(Guid clientId,
        string? productId, int? storeId);

    Task<Either<IServiceError, PriceAlertId>> AddPriceAlertAsync(
        Guid clientId,
        string productId,
        int storeId,
        int priceThreshold
    );

    Task<Either<AlertFetchingError, PriceAlert>> RemovePriceAlertAsync(
        Guid clientId,
        string alertId
    );
}
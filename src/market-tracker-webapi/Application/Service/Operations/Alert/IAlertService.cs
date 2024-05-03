using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Alert;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Alert;

public interface IAlertService
{
    Task<Either<IServiceError, CollectionOutputModel<PriceAlert>>> GetPriceAlertsByClientIdAsync(Guid clientId,
        string? productId);

    Task<Either<IServiceError, StringIdOutputModel>> AddPriceAlertAsync(
        Guid clientId,
        string productId,
        int priceThreshold
    );

    Task<Either<AlertFetchingError, PriceAlert>> RemovePriceAlertAsync(
        Guid clientId,
        string alertId
    );
}
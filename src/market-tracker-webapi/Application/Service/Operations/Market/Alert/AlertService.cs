using market_tracker_webapi.Application.Domain.Models.Market.Price;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Repository.Operations.Account.Users.Client;
using market_tracker_webapi.Application.Repository.Operations.Market.Alert;
using market_tracker_webapi.Application.Repository.Operations.Market.Inventory.Product;
using market_tracker_webapi.Application.Repository.Operations.Market.Price;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Alert;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Alert;

public class AlertService(
    ITransactionManager transactionManager,
    IProductRepository productRepository,
    IPriceRepository priceRepository,
    IPriceAlertRepository priceAlertRepository,
    IClientRepository clientRepository) : IAlertService
{
    public async Task<Either<IServiceError, CollectionOutputModel<PriceAlert>>> GetPriceAlertsByClientIdAsync(
        Guid clientId, string? productId, int? storeId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var priceAlerts
                = await priceAlertRepository.GetPriceAlertsAsync(clientId, productId, storeId);

            return EitherExtensions.Success<IServiceError, CollectionOutputModel<PriceAlert>>(
                new CollectionOutputModel<PriceAlert>(priceAlerts)
            );
        });
    }

    public async Task<Either<IServiceError, StringIdOutputModel>> AddPriceAlertAsync(Guid clientId, string productId,
        int storeId, int priceThreshold)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await productRepository.GetProductByIdAsync(productId) is null)
            {
                return EitherExtensions.Failure<IServiceError, StringIdOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var availabilityStatus = await priceRepository.GetStoreAvailabilityStatusAsync(productId, storeId);
            if (availabilityStatus is null || !availabilityStatus.IsAvailable)
            {
                return EitherExtensions.Failure<IServiceError, StringIdOutputModel>(
                    new ProductFetchingError.OutOfStockInStore(productId, storeId)
                );
            }

            if (await priceAlertRepository.GetPriceAlertAsync(clientId, productId, storeId) is not null)
            {
                return EitherExtensions.Failure<IServiceError, StringIdOutputModel>(
                    new AlertCreationError.ProductAlreadyHasPriceAlertInStore(productId, storeId)
                );
            }

            var deviceTokens = (await clientRepository.GetDeviceTokensByClientIdAsync(clientId)).ToList();
            if (deviceTokens.Count == 0)
            {
                return EitherExtensions.Failure<IServiceError, StringIdOutputModel>(
                    new AlertCreationError.NoDeviceTokensFound(clientId)
                );
            }

            var priceAlert = await priceAlertRepository.AddPriceAlertAsync(
                clientId,
                productId,
                storeId,
                priceThreshold
            );

            return EitherExtensions.Success<IServiceError, StringIdOutputModel>(new StringIdOutputModel(priceAlert.Id));
        });
    }

    public async Task<Either<AlertFetchingError, PriceAlert>> RemovePriceAlertAsync(Guid clientId, string alertId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var priceAlert = await priceAlertRepository.RemovePriceAlertAsync(alertId);

            if (priceAlert is null)
            {
                return EitherExtensions.Failure<AlertFetchingError, PriceAlert>(
                    new AlertFetchingError.AlertByIdNotFound(alertId)
                );
            }

            if (priceAlert.ClientId != clientId)
            {
                return EitherExtensions.Failure<AlertFetchingError, PriceAlert>(
                    new AlertFetchingError.ClientDoesNotOwnAlert(clientId, alertId)
                );
            }

            return EitherExtensions.Success<AlertFetchingError, PriceAlert>(priceAlert);
        });
    }
}
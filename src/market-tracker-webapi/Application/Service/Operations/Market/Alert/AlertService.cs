using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Repository.Market.Alert;
using market_tracker_webapi.Application.Repository.Market.Inventory.Product;
using market_tracker_webapi.Application.Repository.Market.Price;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Alert;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Transaction;

namespace market_tracker_webapi.Application.Service.Operations.Market.Alert;

public class AlertService(
    ITransactionManager transactionManager,
    IProductRepository productRepository,
    IPriceRepository priceRepository,
    IPriceAlertRepository priceAlertRepository,
    IClientDeviceRepository clientDeviceRepository
) : IAlertService
{
    public async Task<IEnumerable<PriceAlert>> GetPriceAlertsByClientIdAsync(
        Guid clientId, string? productId, int? storeId)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await priceAlertRepository.GetPriceAlertsAsync(clientId, productId, storeId)
        );
    }

    public async Task<PriceAlertId> AddPriceAlertAsync(Guid clientId, string productId,
        int storeId, int priceThreshold)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await productRepository.GetProductByIdAsync(productId) is null)
            {
                throw new MarketTrackerServiceException(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            var availabilityStatus = await priceRepository.GetStoreAvailabilityStatusAsync(productId, storeId);
            if (availabilityStatus is null || !availabilityStatus.IsAvailable)
            {
                throw new MarketTrackerServiceException(
                    new ProductFetchingError.OutOfStockInStore(productId, storeId)
                );
            }

            if (await priceAlertRepository.GetPriceAlertAsync(clientId, productId, storeId) is not null)
            {
                throw new MarketTrackerServiceException(
                    new AlertCreationError.ProductAlreadyHasPriceAlertInStore(productId, storeId)
                );
            }

            var deviceTokens =
                (await clientDeviceRepository.GetDeviceTokensByClientIdAsync(clientId)).ToList();

            if (deviceTokens.Count == 0)
            {
                throw new MarketTrackerServiceException(
                    new AlertCreationError.NoDeviceTokensFound(clientId)
                );
            }

            return await priceAlertRepository.AddPriceAlertAsync(
                clientId,
                productId,
                storeId,
                priceThreshold
            );
        });
    }

    public async Task<PriceAlert> RemovePriceAlertAsync(Guid clientId, string alertId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var priceAlert = await priceAlertRepository.RemovePriceAlertByIdAsync(alertId);

            if (priceAlert is null)
            {
                throw new MarketTrackerServiceException(
                    new AlertFetchingError.AlertByIdNotFound(alertId)
                );
            }

            if (priceAlert.ClientId != clientId)
            {
                throw new MarketTrackerServiceException(
                    new AlertFetchingError.ClientDoesNotOwnAlert(clientId, alertId)
                );
            }

            return priceAlert;
        });
    }
}
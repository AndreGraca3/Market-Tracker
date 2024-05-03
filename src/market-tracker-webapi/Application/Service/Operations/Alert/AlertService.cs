using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Repository.Operations.Alert;
using market_tracker_webapi.Application.Repository.Operations.Client;
using market_tracker_webapi.Application.Repository.Operations.Product;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Alert;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.External;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Alert;

public class AlertService(
    ITransactionManager transactionManager,
    IProductRepository productRepository,
    IPriceAlertRepository priceAlertRepository,
    IClientRepository clientRepository,
    INotificationService notificationService) : IAlertService
{
    public async Task<Either<IServiceError, CollectionOutputModel<PriceAlert>>> GetPriceAlertsByClientIdAsync(
        Guid clientId,
        string? productId)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var priceAlerts
                = await priceAlertRepository.GetPriceAlertsByClientIdAsync(clientId, productId);

            return EitherExtensions.Success<IServiceError, CollectionOutputModel<PriceAlert>>(
                new CollectionOutputModel<PriceAlert>(priceAlerts)
            );
        });
    }

    public async Task<Either<IServiceError, StringIdOutputModel>> AddPriceAlertAsync(Guid clientId, string productId,
        int priceThreshold)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await productRepository.GetProductByIdAsync(productId) is null)
            {
                return EitherExtensions.Failure<IServiceError, StringIdOutputModel>(
                    new ProductFetchingError.ProductByIdNotFound(productId)
                );
            }

            if (await priceAlertRepository.GetPriceAlertByClientIdAndProductIdAsync(clientId, productId) is not null)
            {
                return EitherExtensions.Failure<IServiceError, StringIdOutputModel>(
                    new AlertCreationError.ProductAlreadyHasPriceAlert(productId)
                );
            }

            var deviceTokens = (await clientRepository.GetDeviceTokensAsync(clientId)).ToList();
            if (deviceTokens.Count == 0)
            {
                return EitherExtensions.Failure<IServiceError, StringIdOutputModel>(
                    new AlertCreationError.NoDeviceTokensFound(clientId)
                );
            }

            var priceAlert = await priceAlertRepository.AddPriceAlertAsync(
                clientId,
                productId,
                priceThreshold
            );

            await notificationService.SubscribeTokensToTopicAsync(
                deviceTokens.Select(dT => dT.Token).ToList(), productId
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

            var deviceTokens = (await clientRepository.GetDeviceTokensAsync(clientId)).ToList();
            await notificationService.UnsubscribeTokensFromTopicAsync(
                deviceTokens.Select(dT => dT.Token).ToList(), priceAlert.ProductId
            );

            return EitherExtensions.Success<AlertFetchingError, PriceAlert>(priceAlert);
        });
    }
}
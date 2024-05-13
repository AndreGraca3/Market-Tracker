using market_tracker_webapi.Application.Service.Errors.Alert;

namespace market_tracker_webapi.Application.Http.Problems;

public class AlertProblem(
    int status,
    string subtype,
    string title,
    string detail,
    object? data = null
) : Problems.Problem(status, subtype, title, detail, data)
{
    public class NoDeviceTokensFound(AlertCreationError.NoDeviceTokensFound data) : AlertProblem(
        409,
        "device-tokens-not-found",
        "Device tokens not found",
        "Client does not have at least 1 device registered to be notified",
        data
    );

    public class AlertByIdNotFound(AlertFetchingError.AlertByIdNotFound data) : AlertProblem(
        404,
        "alert-by-id-not-found",
        "Alert by id not found",
        "Alert with the given id not found",
        data
    );

    public class ClientDoesNotOwnAlert(AlertFetchingError.ClientDoesNotOwnAlert data) : AlertProblem(
        403,
        "client-does-not-own-alert",
        "Client does not own alert",
        "Client does not own the alert",
        data
    );

    public class ProductAlreadyHasPriceAlert(AlertCreationError.ProductAlreadyHasPriceAlertInStore data) : AlertProblem(
        409,
        "product-already-has-price-alert",
        "Product already has price alert",
        "Product already has a price alert",
        data
    );

    public static AlertProblem FromServiceError(IAlertError error)
    {
        return error switch
        {
            AlertCreationError.NoDeviceTokensFound noDeviceTokensFound => new NoDeviceTokensFound(noDeviceTokensFound),
            AlertCreationError.ProductAlreadyHasPriceAlertInStore productAlreadyHasPriceAlertInStore =>
                new ProductAlreadyHasPriceAlert(productAlreadyHasPriceAlertInStore),
            AlertFetchingError.AlertByIdNotFound alertByIdNotFound => new AlertByIdNotFound(alertByIdNotFound),
            AlertFetchingError.ClientDoesNotOwnAlert clientDoesNotOwnAlert => new ClientDoesNotOwnAlert(
                clientDoesNotOwnAlert),
            _ => throw new ArgumentOutOfRangeException(nameof(error))
        };
    }
}
using market_tracker_webapi.Application.Service.Errors.Alert;

namespace market_tracker_webapi.Application.Http.Problem;

public class AlertProblem(
    int status,
    string subtype,
    string title,
    string detail,
    object? data = null
) : Problem(status, subtype, title, detail, data)
{
    public class NoDeviceTokensFound(AlertCreationError.NoDeviceTokensFound data) : UserProblem(
        409,
        "device-tokens-not-found",
        "Device tokens not found",
        "Client does not have at least 1 device registered to be notified",
        data
    );

    public class AlertByIdNotFound(AlertFetchingError.AlertByIdNotFound data) : UserProblem(
        404,
        "alert-by-id-not-found",
        "Alert by id not found",
        "Alert with the given id not found",
        data
    );
    
    public class ClientDoesNotOwnAlert(AlertFetchingError.ClientDoesNotOwnAlert data) : UserProblem(
        403,
        "client-does-not-own-alert",
        "Client does not own alert",
        "Client does not own the alert",
        data
    );
    
    public class ProductAlreadyHasPriceAlert(AlertCreationError.ProductAlreadyHasPriceAlertInStore data) : UserProblem(
        409,
        "product-already-has-price-alert",
        "Product already has price alert",
        "Product already has a price alert",
        data
    );
}
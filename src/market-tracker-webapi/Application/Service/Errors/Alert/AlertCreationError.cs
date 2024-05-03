namespace market_tracker_webapi.Application.Service.Errors.Alert;

public class AlertCreationError : IAlertError
{
    public class ProductAlreadyHasPriceAlert(string productId) : AlertCreationError
    {
        public string ProductId { get; } = productId;
    }
    
    public class NoDeviceTokensFound(Guid clientId) : AlertCreationError
    {
        public Guid ClientId { get; } = clientId;
    }
}
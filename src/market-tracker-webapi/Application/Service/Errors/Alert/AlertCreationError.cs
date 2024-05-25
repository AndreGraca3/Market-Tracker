namespace market_tracker_webapi.Application.Service.Errors.Alert;

public abstract class AlertCreationError : IAlertError
{
    public class ProductAlreadyHasPriceAlertInStore(string productId, int storeId) : AlertCreationError
    {
        public string ProductId { get; } = productId;
        public int StoreId { get; } = storeId;
    }
    
    public class NoDeviceTokensFound(Guid clientId) : AlertCreationError
    {
        public Guid ClientId { get; } = clientId;
    }
}
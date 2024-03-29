namespace market_tracker_webapi.Application.Service.Errors.Store;

public class StoreCreationError : IStoreError
{
    public class StoreAddressAlreadyExists(string storeName) : StoreCreationError
    {
        public string Name { get; } = storeName;
    }
    
}
namespace market_tracker_webapi.Application.Service.Errors.Store;

public class StoreCreationError : IStoreError
{
    public class StoreNameAlreadyExists(string storeName) : StoreCreationError
    {
        public string StoreName { get; } = storeName;
    }
    
}
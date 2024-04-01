namespace market_tracker_webapi.Application.Service.Errors.Store;

public class StoreCreationError : IStoreError
{
    public class StoreAddressAlreadyExists(string address) : StoreCreationError
    {
        public string Address { get; } = address;
    }
    
    public class StoreNameAlreadyExists(string storeName) : StoreCreationError
    {
        public string StoreName { get; } = storeName;
    }
    
}
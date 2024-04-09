namespace market_tracker_webapi.Application.Service.Errors.Store;

public class StoreFetchingError : IStoreError
{
    public class StoreByIdNotFound(int id) : StoreFetchingError
    {
        public int Id { get; } = id;
    }
}
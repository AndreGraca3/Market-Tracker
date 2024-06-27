using market_tracker_webapi.Application.Service.Errors.Store;

namespace market_tracker_webapi.Application.Http.Problems;

public class StoreProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class StoreByIdNotFound(StoreFetchingError.StoreByIdNotFound data)
        : CompanyProblem(
            404,
            "store-not-found",
            "Store not found",
            $"Store with id {data.Id} not found",
            data
        );

    public class StoreNameAlreadyExists(StoreCreationError.StoreNameAlreadyExists data)
        : CompanyProblem(
            400,
            "store-name-already-exists",
            "Store name already exists",
            $"Store with name {data.StoreName} already exists",
            data
        );

    public class StoreByOperatorIdNotFound(StoreFetchingError.StoreByOperatorIdNotFound data)
        : CompanyProblem(
            404,
            "store-by-operator-not-found",
            "Store by operator not found",
            "Operator has no store assigned yet",
            data
        );

    public static CompanyProblem FromServiceError(IStoreError error)
        => error switch
        {
            StoreFetchingError.StoreByIdNotFound storeByIdNotFound => new StoreByIdNotFound(storeByIdNotFound),
            StoreCreationError.StoreNameAlreadyExists storeNameAlreadyExists => new StoreNameAlreadyExists(
                storeNameAlreadyExists),
            StoreFetchingError.StoreByOperatorIdNotFound storeByOperatorIdNotFound => new StoreByOperatorIdNotFound(
                storeByOperatorIdNotFound),
            _ => throw new ArgumentOutOfRangeException(nameof(error))
        };
}
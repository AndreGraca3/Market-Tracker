using market_tracker_webapi.Application.Service.Errors.Product;

namespace market_tracker_webapi.Application.Http.Problem;

public class ProductProblem(
    int status,
    string subType,
    string title,
    string detail,
    object? data = null
) : Problem(status, subType, title, detail, data)
{
    public class ProductByIdNotFound(ProductFetchingError.ProductByIdNotFound data)
        : ProductProblem(
            404,
            "product-not-found",
            "Product not found",
            $"Product with id {data.Id} not found",
            data
        );

    public class OutOfStockInStore(ProductFetchingError.OutOfStockInStore data)
        : ProductProblem(
            409,
            "product-out-of-stock",
            "Product out of stock",
            "This store does not have that product in stock",
            data
        );

    public class ProductNotFoundInStore(ProductFetchingError.ProductNotFoundInStore data)
        : ProductProblem(
            404,
            "product-not-in-store",
            "Product not in store",
            "This store does not have that product",
            data
        );
}
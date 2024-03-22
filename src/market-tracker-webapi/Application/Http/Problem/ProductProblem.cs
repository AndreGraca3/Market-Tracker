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
            $"Product with id {data.Id} not found"
        ) { }
    
    public class ProductByBrandNotFound(ProductFetchingError.ProductByBrandNotFound data)
        : ProductProblem(
            404,
            "product-not-found",
            "Product not found",
            $"Product with brand id {data.BrandId} not found"
        ) { }
}

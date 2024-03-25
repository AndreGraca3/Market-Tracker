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

    public class InvalidBrand(ProductCreationError.InvalidBrand data)
        : ProductProblem(
            400,
            "invalid-brand",
            "Invalid brand",
            $"Brand with name {data.BrandName} not found",
            data
        );

    public class ProductAlreadyExists(ProductCreationError.ProductAlreadyExists data)
        : ProductProblem(
            409,
            "product-already-exists",
            "Product already exists",
            $"Product with id {data.ProductId} already exists",
            data
        );
}

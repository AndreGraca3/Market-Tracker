using market_tracker_webapi.Application.Domain.Models.Account.Users;
using market_tracker_webapi.Application.Domain.Models.Market.Inventory.Product;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;

public record ProductReviewOutputModel(ClientItem Client, ProductReview Review);

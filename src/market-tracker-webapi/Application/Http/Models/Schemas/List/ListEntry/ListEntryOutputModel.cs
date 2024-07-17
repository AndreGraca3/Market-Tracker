using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product.Offer;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;

namespace market_tracker_webapi.Application.Http.Models.Schemas.List.ListEntry;

using ListEntry = Domain.Schemas.List.ListEntry;

public record ListEntryOutputModel(string Id, string ProductId, StoreItemOutputModel? StoreId, int Quantity);

public record ListEntryOfferOutputModel(string Id, ProductOfferOutputModel ProductOffer, int Quantity);

public static class ListEntryOutputModelMapper
{
    public static ListEntryOutputModel ToOutputModel(this ListEntry listEntry)
    {
        return new ListEntryOutputModel(listEntry.Id.Value, listEntry.Product.Id.Value,
            listEntry.Store?.ToOutputModel(), listEntry.Quantity);
    }

    public static ListEntryOfferOutputModel ToOutputModel(this ListEntryOffer listEntryOffer)
    {
        return new ListEntryOfferOutputModel(
            listEntryOffer.Id,
            listEntryOffer.ProductOffer.ToOutputModel(),
            listEntryOffer.Quantity
        );
    }
}
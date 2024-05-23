using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.List.ListEntry;

public record ListEntryCreationInputModel(
    [Required] string ProductId,
    [Required] int StoreId,
    [Required] int Quantity
);
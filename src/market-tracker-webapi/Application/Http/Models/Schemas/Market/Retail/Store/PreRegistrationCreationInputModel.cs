using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;

public record PreRegistrationCreationInputModel(
    [Required] [MaxLength(30)] string OperatorName,
    [Required] [MaxLength(200)] string Email,
    [Required]
    [Range(210000000, 999999999)]
    int PhoneNumber,
    [Required] [MaxLength(30)] string StoreName,
    [Required] [MaxLength(30)] string StoreAddress,
    [Required] [MaxLength(30)] string CompanyName,
    [Required] string CompanyLogoUrl,
    [MaxLength(30)] string? CityName,
    [Required] string Document
);
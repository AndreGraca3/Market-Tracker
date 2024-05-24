using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Company;

public record CompanyCreationInputModel(
    [Required] [MaxLength(30)] string CompanyName,
    [Required] string CompanyLogoUrl
);
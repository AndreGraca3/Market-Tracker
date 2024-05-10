using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Company;

public class CompanyCreationInputModel
{
    [Required]
    public string CompanyName { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Company;

public class CompanyCreationInputModel
{
    [Required]
    public string CompanyName { get; set; }
}

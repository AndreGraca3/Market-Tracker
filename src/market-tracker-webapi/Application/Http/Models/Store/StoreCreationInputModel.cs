using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Store;

public class StoreCreationInputModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Address { get; set; }

    public int? CityId { get; set; }

    [Required]
    public int CompanyId { get; set; }
}

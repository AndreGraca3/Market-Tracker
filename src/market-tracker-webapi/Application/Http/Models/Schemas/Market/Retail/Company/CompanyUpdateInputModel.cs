using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Company;

public record CompanyUpdateInputModel([Required] [MaxLength(30)] string CompanyName);
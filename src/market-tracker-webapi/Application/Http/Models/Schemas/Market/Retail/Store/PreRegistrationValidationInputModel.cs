using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;

public record PreRegistrationValidationInputModel([Required] bool IsApproved);
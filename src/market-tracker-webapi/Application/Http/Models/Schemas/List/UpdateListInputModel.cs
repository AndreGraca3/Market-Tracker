using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.List;

public record UpdateListInputModel([MaxLength(30)] string? ListName, bool? IsArchived);
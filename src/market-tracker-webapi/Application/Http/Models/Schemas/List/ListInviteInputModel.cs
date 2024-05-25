using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.List;

public record ListInviteInputModel([Required] Guid ClientId);
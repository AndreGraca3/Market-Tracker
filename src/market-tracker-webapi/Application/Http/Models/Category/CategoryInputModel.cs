using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Category;

public record CategoryInputModel([Required] [MaxLength(50)] string Name);

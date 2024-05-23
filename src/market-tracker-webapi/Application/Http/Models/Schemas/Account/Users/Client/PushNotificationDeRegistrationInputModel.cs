using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Client;

public record PushNotificationDeRegistrationInputModel([Required] string DeviceId);
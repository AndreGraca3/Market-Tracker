using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Client;

public record PushNotificationRegistrationInputModel(
    [Required] string DeviceId,
    [Required] string FirebaseToken
);
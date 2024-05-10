using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Client;

public class PushNotificationDeRegistrationInputModel
{
    [Required] public string DeviceId { get; set; }
}
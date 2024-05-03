using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Client;

public class PushNotificationDeRegistrationInputModel
{
    [Required] public string DeviceId { get; set; }
}
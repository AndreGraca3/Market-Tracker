using System.ComponentModel.DataAnnotations;

namespace market_tracker_webapi.Application.Http.Models.Client;

public class PushNotificationRegistrationInputModel
{
    [Required] public string DeviceId { get; set; }
    
    [Required] public string FirebaseToken { get; set; }
}
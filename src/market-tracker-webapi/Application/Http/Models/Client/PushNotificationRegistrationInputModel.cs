namespace market_tracker_webapi.Application.Http.Models.Client;

public class PushNotificationRegistrationInputModel
{
    public string DeviceId { get; set; }
    
    public string FirebaseToken { get; set; }
}
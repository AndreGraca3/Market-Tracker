namespace market_tracker_webapi.Application.Service.External;

public interface INotificationSenderService
{
    Task SendNotificationAsync(string title, string body, string token);
    
    Task SendNotificationAsync(string title, string body, List<string> tokens);
}
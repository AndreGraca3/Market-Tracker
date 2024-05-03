namespace market_tracker_webapi.Application.Service.External;

public interface INotificationService
{
    Task<bool> SendNotificationToTokenAsync(string title, string body, string token);

    Task<bool> SendNotificationToTokensAsync(string title, string body, List<string> tokens);

    Task<int> SubscribeTokensToTopicAsync(List<string> token, string topic);

    Task<int> UnsubscribeTokensFromTopicAsync(List<string> token, string topic);
    
    Task<bool> SendNotificationToTopicAsync(string title, string body, string topic);
}
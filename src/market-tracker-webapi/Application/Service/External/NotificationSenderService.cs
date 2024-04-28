using FirebaseAdmin.Messaging;

namespace market_tracker_webapi.Application.Service.External;

public class NotificationSenderService : INotificationSenderService
{
    public async Task SendNotificationAsync(string title, string body, string token)
    {
        var message = new Message()
        {
            Notification = new Notification
            {
                Title = "Test",
                Body = "Test body"
            },
            Data = new Dictionary<string, string>()
            {
                ["FirstName"] = "John",
                ["LastName"] = "Doe"
            },
            Token = token
        };
        await FirebaseMessaging.DefaultInstance.SendAsync(message);
    }

    public Task SendNotificationAsync(string title, string body, List<string> tokens)
    {
        var message = new MulticastMessage()
        {
            Notification = new Notification
            {
                Title = "Test",
                Body = "Test body"
            },
            Data = new Dictionary<string, string>()
            {
                ["FirstName"] = "John",
                ["LastName"] = "Doe"
            },
            Tokens = tokens
        };
        return FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
    }
}
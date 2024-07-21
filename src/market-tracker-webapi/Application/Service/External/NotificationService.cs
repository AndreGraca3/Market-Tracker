using System.Diagnostics.CodeAnalysis;
using FirebaseAdmin.Messaging;

namespace market_tracker_webapi.Application.Service.External;

[ExcludeFromCodeCoverage]
public class NotificationService : INotificationService
{
    public async Task<bool> SendNotificationToTokenAsync(string title, string body, string token)
    {
        var message = new Message
        {
            Notification = new Notification
            {
                Title = title,
                Body = body
            },
            Token = token
        };
        var mId = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        return mId != null;
    }

    public async Task<bool> SendNotificationToTokensAsync(string title, string body, List<string> tokens)
    {
        var message = new MulticastMessage
        {
            Notification = new Notification
            {
                Title = title,
                Body = body
            },
            Tokens = tokens
        };
        var mId = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
        return mId != null;
    }

    public async Task<int> SubscribeTokensToTopicAsync(List<string> tokens, string topic)
    {
        var topicManagementRes =
            await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(tokens, topic);
        return topicManagementRes.SuccessCount;
    }

    public async Task<int> UnsubscribeTokensFromTopicAsync(List<string> tokens, string topic)
    {
        var topicManagementRes =
            await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(tokens, topic);
        return topicManagementRes.SuccessCount;
    }

    public async Task<bool> SendNotificationToTopicAsync(string title, string body, string topic)
    {
        var message = new Message
        {
            Notification = new Notification
            {
                Title = title,
                Body = body
            },
            Topic = topic
        };

        var mId = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        return mId != null;
    }
}
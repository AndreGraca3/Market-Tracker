namespace market_tracker_webapi.Application.Service.Errors.Alert;

public abstract class AlertFetchingError : IAlertError
{
    public class AlertByIdNotFound(string alertId) : AlertFetchingError
    {
        public string AlertId { get; } = alertId;
    }
    
    public class ClientDoesNotOwnAlert(Guid clientId, string alertId) : AlertFetchingError
    {
        public Guid ClientId { get; } = clientId;
        public string AlertId { get; } = alertId;
    }
}
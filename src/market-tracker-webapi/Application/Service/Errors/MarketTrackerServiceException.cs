namespace market_tracker_webapi.Application.Service.Errors;

public class MarketTrackerServiceException(IServiceError serviceError) : Exception
{
    public IServiceError ServiceError { get; } = serviceError;
}
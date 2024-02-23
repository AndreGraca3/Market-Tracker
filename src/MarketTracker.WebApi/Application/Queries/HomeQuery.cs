namespace MarketTracker.WebApi.Application.Queries
{
    public class HomeQuery : IHomeQuery
    {
        public string GetHome()
        {
            return "Welcome to Market Tracker!";
        }
    }
}

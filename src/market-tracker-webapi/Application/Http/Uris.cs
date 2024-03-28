namespace market_tracker_webapi.Application.Http;

public class Uris
{
    const string ApiBase = "/api";
    
    public static class Users
    {
        // TODO: Digo issue
    }
    
    public static class Products
    {
        public const string Base = $"{ApiBase}/products";
        public const string ProductById = $"{Base}/{{id}}";
    }
    
    public static class Companies
    {
        public const string Base = $"{ApiBase}/companies";
        public const string CompanyById = $"{Base}/{{id}}";
    }
    
    public static class Cities
    {
        public const string Base = $"{ApiBase}/cities";
        public const string CityById = $"{Base}/{{id}}";
    }
}
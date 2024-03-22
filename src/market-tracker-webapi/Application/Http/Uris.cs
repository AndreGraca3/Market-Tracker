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

    public static class Categories
    {
        public const string Base = $"{ApiBase}/categories";
        public const string CategoryById = $"{Base}/{{id}}";
    }
}

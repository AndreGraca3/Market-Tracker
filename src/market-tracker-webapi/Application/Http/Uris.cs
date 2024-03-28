using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http;

public class Uris
{
    const string ApiBase = "/api";
    
    public static class Users
    {
        public const string Base = $"{ApiBase}/users";
        public const string UserById = $"{Base}/{{id}}";
    }
    
    public static class Products
    {
        public const string Base = $"{ApiBase}/products";
        public const string ProductById = $"{Base}/{{id}}";
    }
}
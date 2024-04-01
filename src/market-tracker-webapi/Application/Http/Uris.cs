using System.Text.RegularExpressions;

namespace market_tracker_webapi.Application.Http;

public static class Uris
{
    public const string ApiBase = "/api";

    public static class Users
    {
        public const string Base = $"{ApiBase}/users";
        public const string UserById = $"{Base}/{{id}}";

        public static string BuildUserByIdUri(Guid id) => $"{Base}/{id}";
    }

    public static class Tokens
    {
        public const string Base = $"{ApiBase}/tokens";
    }

    public static class Products
    {
        public const string Base = $"{ApiBase}/products";
        public const string ProductById = $"{Base}/{{id}}";

        public static string BuildProductByIdUri(int id) => ProductById.ExpandUri(id);
    }

    public static class Categories
    {
        public const string Base = $"{ApiBase}/categories";
        public const string CategoryById = $"{Base}/{{id}}";

        public static string BuildCategoryByIdUri(int id) => CategoryById.ExpandUri(id);
    }

    private static string ExpandUri(this string input, params object[] args)
    {
        var result = input;
        var argIndex = 0;
        result = Regex.Replace(
            result,
            @"\{(.*?)\}",
            _ =>
                args[argIndex++].ToString()
                ?? throw new ArgumentException(
                    "Not enough arguments provided to replace all placeholders."
                )
        );
        return result;
    }
}

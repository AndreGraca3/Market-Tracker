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

    public static class Auth
    {
        public const string Base = $"{ApiBase}/auth";
        public const string GoogleAuth = $"{ApiBase}/google-signin";
    }

    public static class Clients
    {
        public const string Base = $"{ApiBase}/clients";
        public const string ClientById = $"{Base}/{{id}}";
        public const string Me = $"{Base}/me";
    }

    public static class Operator
    {
        public const string Base = $"{ApiBase}/operators";
        public const string OperatorById = $"{Base}/{{id}}";
    }

    public static class Products
    {
        public const string Base = $"{ApiBase}/products";
        public const string ProductById = $"{Base}/{{productId}}";
        public const string ReviewsByProductId = $"{ProductById}/reviews";
        public const string StatsByProductId = $"{ProductById}/stats";
        public const string PricesByProductId = $"{ProductById}/prices";
        public const string HistoryByProductId = $"{ProductById}/price-history";
        public const string ProductPreferencesById = $"{ProductById}/me";

        public static string BuildProductByIdUri(string id) => ProductById.ExpandUri(id);

        public static string BuildReviewsByProductIdUri(string id) =>
            ReviewsByProductId.ExpandUri(id);
    }

    public static class Lists
    {
        public const string Base = $"{ApiBase}/lists";
        public const string ListById = $"{Base}/{{listId}}";
        public const string ListProductsByListId = $"{ListById}/products";
        public const string ListEntriesByListIdAndProductId = $"{ListProductsByListId}/{{productId}}";
        public const string Clients = $"{ListById}/clients";

        public static string BuildListByIdUri(int id) => ListById.ExpandUri(id);

        public static string BuildListProductsByListIdUri(int id) => ListProductsByListId.ExpandUri(id);

        public static string BuildListProductsByListIdAndProductIdUri(int listId, string productId) =>
            ListEntriesByListIdAndProductId.ExpandUri(listId, productId);
    }

    public static class Categories
    {
        public const string Base = $"{ApiBase}/categories";
        public const string CategoryById = $"{Base}/{{categoryId}}";

        public static string BuildCategoryByIdUri(int id) => CategoryById.ExpandUri(id);
    }

    public static class Companies
    {
        public const string Base = $"{ApiBase}/companies";
        public const string CompanyById = $"{Base}/{{id}}";

        public static string BuildCompanyByIdUri(int id) => CompanyById.ExpandUri(id);
    }

    public static class Cities
    {
        public const string Base = $"{ApiBase}/cities";
        public const string CityById = $"{Base}/{{id}}";

        public static string BuildCategoryByIdUri(int id) => CityById.ExpandUri(id);
    }

    public static class Stores
    {
        public const string Base = $"{ApiBase}/stores";
        public const string StoreById = $"{Base}/{{id}}";
        public const string StoresFromCompany = $"{Base}/company/{{companyId}}";
        public const string StoresByCityName = $"{Base}/city/{{cityName}}";
        public const string StoresPreRegister = $"${Base}/pre-register";
        public const string StoresPending = $"${Base}/pending";
        public const string StoresPendingById = $"${Base}/pending/{{id}}";
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
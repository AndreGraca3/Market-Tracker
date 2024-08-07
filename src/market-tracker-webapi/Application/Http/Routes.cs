using System.Text.RegularExpressions;

namespace market_tracker_webapi.Application.Http;

public static class Uris
{
    public const string JsonMediaType = "application/json";

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
        public const string Login = $"{Base}/sign-in";
        public const string Logout = $"{Base}/sign-out";
        public const string GoogleAuth = $"{Base}/google-sign-in";
    }

    public static class Clients
    {
        public const string Base = $"{ApiBase}/clients";
        public const string ClientById = $"{Base}/{{id}}";
        public const string Me = $"{Base}/me";
        public const string RegisterPushNotifications = $"{Me}/register-push-notifications";
        public const string DeRegisterPushNotifications = $"{Me}/deregister-push-notifications";
    }

    public static class Operator
    {
        public const string Base = $"{ApiBase}/operators";
        public const string OperatorById = $"{Base}/{{id}}";

        public static string BuildOperatorByIdUri(Guid id) => OperatorById.ExpandUri(id);
    }

    public static class Products
    {
        public const string Base = $"{ApiBase}/products";
        public const string ProductById = $"{Base}/{{productId}}";
        public const string AvailabilityByProductId = $"{ProductById}/availability";
        public const string ReviewsByProductId = $"{ProductById}/reviews";
        public const string StatsByProductId = $"{ProductById}/stats";
        public const string PricesByProductId = $"{ProductById}/prices";
        public const string HistoryByProductId = $"{ProductById}/price-history/{{storeId}}";
        public const string ProductPreferencesById = $"{ProductById}/me";
        public const string Favourites = $"{Base}/favourites";

        public static string BuildProductByIdUri(string id) => ProductById.ExpandUri(id);
    }

    public static class Alerts
    {
        public const string Base = $"{ApiBase}/alerts";
        public const string AlertById = $"{Base}/{{alertId}}";

        public static string BuildAlertByIdUri(string id) => AlertById.ExpandUri(id);
    }

    public static class Lists
    {
        public const string Base = $"{ApiBase}/lists";
        public const string ListById = $"{Base}/{{listId}}";
        public const string EntriesByListId = $"{ListById}/entries";
        public const string ListEntryEntryById = $"{EntriesByListId}/{{entryId}}";
        public const string ClientsByListId = $"{ListById}/clients";
        public const string ClientByListId = $"{ClientsByListId}/{{clientId}}";

        public static string BuildListByIdUri(string id) => ListById.ExpandUri(id);

        public static string BuildListEntryByIdUri(string listId, string entryId) =>
            ListEntryEntryById.ExpandUri(listId, entryId);
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

        public static string BuildCityByIdUri(int id) => CityById.ExpandUri(id);
    }

    public static class Stores
    {
        public const string Base = $"{ApiBase}/stores";
        public const string StoreById = $"{Base}/{{id}}";
        public const string StoresPreRegister = $"{Base}/pre-register";
        public const string StoresPending = $"{Base}/pending";
        public const string StoresPendingById = $"{Base}/pending/{{id}}";

        public static string BuildStoreByIdUri(int id) => StoreById.ExpandUri(id);
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
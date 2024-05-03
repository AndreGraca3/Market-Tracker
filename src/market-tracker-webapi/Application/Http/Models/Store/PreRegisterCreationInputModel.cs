namespace market_tracker_webapi.Application.Http.Models.Store;

public record PreRegisterCreationInputModel(
    string OperatorName,
    string Email,
    int PhoneNumber,
    string StoreName,
    string StoreAddress,
    string CompanyName,
    string? CityName,
    string Document
);
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;

public record PreRegistrationOutputModel(
    Guid Code,
    string OperatorName,
    string Email,
    int PhoneNumber,
    string StoreName,
    string CompanyName,
    string CompanyLogoUrl,
    string Document,
    DateTime CreatedAt
);

public static class PreRegisterModelMapper
{
    public static PreRegistrationOutputModel ToPreRegisterOutputModel(this PreRegistration preRegistration)
    {
        return new PreRegistrationOutputModel(
            preRegistration.Code.Value,
            preRegistration.OperatorName,
            preRegistration.Email,
            preRegistration.PhoneNumber,
            preRegistration.StoreName,
            preRegistration.CompanyName,
            preRegistration.CompanyLogoUrl,
            preRegistration.Document,
            preRegistration.CreatedAt
        );
    }
}
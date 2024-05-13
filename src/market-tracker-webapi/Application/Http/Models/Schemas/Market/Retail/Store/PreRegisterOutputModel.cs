using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;

using Operator = Operator;

public record PreRegisterOutputModel(
    Operator Operator,
    string StoreName,
    string? CompanyName,
    string Document);

public static class PreRegisterModelMapper
{
    public static PreRegisterOutputModel ToPreRegisterOutputModel(this PreRegistration preRegistration,
        Operator @operator)
    {
        return new PreRegisterOutputModel(
            @operator,
            preRegistration.StoreName,
            preRegistration.CompanyName,
            preRegistration.Document);
    }
}
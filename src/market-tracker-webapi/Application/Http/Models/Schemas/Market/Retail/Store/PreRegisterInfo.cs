using market_tracker_webapi.Application.Domain.Models.Account.Auth;

namespace market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;

using Operator = Domain.Models.Account.Users.Operator;

public record PreRegisterInfo(
    Operator OperatorInfo,
    string StoreName,
    string? CompanyName,
    string Document)
{
    public static PreRegisterInfo ToPreRegisterInfo(PreRegistration preRegistration)
    {
        return new PreRegisterInfo
        (
            new Operator
            (
                preRegistration.Code,
                preRegistration.OperatorName,
                preRegistration.Email,
                preRegistration.PhoneNumber,
                preRegistration.CreatedAt
            ),
            preRegistration.StoreName,
            preRegistration.CompanyName,
            preRegistration.Document
        );
    }
};
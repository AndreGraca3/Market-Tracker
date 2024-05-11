using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto.Operator;

namespace market_tracker_webapi.Application.Http.Models.Store;

public record PreRegisterInfo(
    OperatorInfo OperatorInfo,
    string StoreName,
    string? CompanyName,
    string Document)
{
    public static PreRegisterInfo ToPreRegisterInfo(PreRegistration preRegistration)
    {
        return new PreRegisterInfo
        (
            new OperatorInfo
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
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto.Operator;

namespace market_tracker_webapi.Application.Http.Models.Store;

public record PreRegisterInfo(
    OperatorInfo OperatorInfo,
    string StoreName,
    string? CompanyName,
    string Document)
{
    public static PreRegisterInfo ToPreRegisterInfo(PreRegister preRegister)
    {
        return new PreRegisterInfo
        (
            new OperatorInfo
            (
                preRegister.Code,
                preRegister.OperatorName,
                preRegister.Email,
                preRegister.PhoneNumber,
                preRegister.CreatedAt
            ),
            preRegister.StoreName,
            preRegister.CompanyName,
            preRegister.Document
        );
    }
};
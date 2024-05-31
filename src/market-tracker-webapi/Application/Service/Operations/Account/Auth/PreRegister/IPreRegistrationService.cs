using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Application.Service.Operations.Account.Auth.PreRegister;

public interface IPreRegistrationService
{
    Task<PaginatedResult<OperatorItem>> GetPreRegistrationsAsync(bool? isValid, int skip, int limit);

    Task<PreRegistration> GetPreRegistrationByIdAsync(Guid id);

    Task<PreRegistrationCode> AddPreRegistrationAsync(
        string operatorName,
        string email,
        int phoneNumber,
        string storeName,
        string storeAddress,
        string companyName,
        string companyLogoUrl,
        string? cityName,
        string document
    );

    Task<PreRegistrationCode> UpdatePreRegistrationByIdAsync(
        Guid id,
        bool isApproved
    );
}
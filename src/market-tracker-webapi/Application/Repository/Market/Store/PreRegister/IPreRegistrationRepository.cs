using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Application.Repository.Market.Store.PreRegister;

public interface IPreRegistrationRepository
{
    Task<PaginatedResult<OperatorItem>> GetPreRegistersAsync(bool? isValid, int skip, int take);

    Task<PreRegistration?> GetPreRegisterByIdAsync(Guid id);

    Task<PreRegistration?> GetPreRegisterByEmail(string email);

    Task<PreRegistrationCode> CreatePreRegisterAsync(string operatorName, string email, int phoneNumber,
        string storeName, string storeAddress, string companyName, string companyLogoUrl, string? cityName,
        string document);

    Task<PreRegistration?> UpdatePreRegistrationById(Guid id, bool isApproved);

    Task<PreRegistrationCode?> DeletePreRegisterAsync(Guid id);
}
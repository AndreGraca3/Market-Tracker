using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Models.Account.Auth;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Operator;

namespace market_tracker_webapi.Application.Repository.Market.Store.PreRegister;

public interface IPreRegistrationRepository
{
    Task<PaginatedResult<OperatorOutputModel>> GetPreRegistersAsync(bool? isValid, int skip, int take);

    Task<PreRegistration?> GetPreRegisterByIdAsync(Guid id);

    Task<PreRegistration?> GetPreRegisterByEmail(string email);

    Task<Guid> CreatePreRegisterAsync(string operatorName, string email, int phoneNumber,
        string storeName, string storeAddress, string companyName, string? cityName, string document);

    Task<PreRegistration?> UpdatePreRegistrationById(Guid id, bool isApproved);

    Task<Guid?> DeletePreRegisterAsync(Guid id);
}
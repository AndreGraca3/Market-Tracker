using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Operator;

namespace market_tracker_webapi.Application.Repository.Operations.Market.Store.PreRegister;

public interface IPreRegistrationRepository
{
    Task<PaginatedResult<OperatorItem>> GetPreRegistersAsync(bool? isValid, int skip, int take);

    Task<PreRegistration?> GetPreRegisterByIdAsync(Guid id);

    Task<PreRegistration?> GetPreRegisterByEmail(string email);

    Task<Guid> CreatePreRegisterAsync(string operatorName, string email, int phoneNumber,
        string storeName, string storeAddress, string companyName, string? cityName, string document);

    Task<PreRegistration?> UpdatePreRegistrationById(Guid id, bool isApproved);

    Task<Guid?> DeletePreRegisterAsync(Guid id);
}
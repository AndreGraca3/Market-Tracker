using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Operator;

namespace market_tracker_webapi.Application.Repository.Operations.PreRegister;

using PreRegister = Domain.PreRegister;

public interface IPreRegistrationRepository
{
    Task<PaginatedResult<OperatorItem>> GetPreRegistersAsync(bool? isValid, int skip, int take);

    Task<PreRegister?> GetPreRegisterByIdAsync(Guid id);

    Task<PreRegister?> GetPreRegisterByEmail(string email);

    Task<Guid> CreatePreRegisterAsync(string operatorName, string email, int phoneNumber,
        string storeName, string storeAddress, string companyName, string? cityName, string document);

    Task<PreRegister?> UpdatePreRegistrationById(Guid id, bool isApproved);

    Task<Guid?> DeletePreRegisterAsync(Guid id);
}
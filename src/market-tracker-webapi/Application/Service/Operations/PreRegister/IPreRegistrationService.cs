using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Store;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Operator;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.PreRegister;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.PreRegister;

public interface IPreRegistrationService
{
    Task<Either<IServiceError, PaginatedResult<OperatorItem>>> GetPreRegistrationsAsync(bool? isValid, int skip,
        int limit);

    Task<Either<PreRegistrationFetchingError, PreRegisterInfo>> GetPreRegistrationByIdAsync(Guid id);

    Task<Either<PreRegistrationCreationError, GuidOutputModel>> AddPreRegistrationAsync(
        string operatorName,
        string email,
        int phoneNumber,
        string storeName,
        string storeAddress,
        string companyName,
        string? cityName,
        string document
    );

    Task<Either<PreRegistrationFetchingError, GuidOutputModel>> UpdatePreRegistrationByIdAsync(
        Guid id,
        bool isApproved
    );
}
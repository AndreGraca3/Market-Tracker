using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Operator;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.PreRegister;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Account.Auth.PreRegister;

public interface IPreRegistrationService
{
    Task<Either<IServiceError, PaginatedResult<OperatorOutputModel>>> GetPreRegistrationsAsync(bool? isValid, int skip,
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
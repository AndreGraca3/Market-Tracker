using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Models.Account.Auth;
using market_tracker_webapi.Application.Domain.Models.Account.Users;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Operator;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.PreRegister;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Account.Auth.PreRegister;

public interface IPreRegistrationService
{
    Task<Either<IServiceError, PaginatedResult<OperatorItem>>> GetPreRegistrationsAsync(bool? isValid, int skip,
        int limit);

    Task<Either<PreRegistrationFetchingError, PreRegistration>> GetPreRegistrationByIdAsync(Guid id);

    Task<Either<PreRegistrationCreationError, PreRegistrationId>> AddPreRegistrationAsync(
        string operatorName,
        string email,
        int phoneNumber,
        string storeName,
        string storeAddress,
        string companyName,
        string? cityName,
        string document
    );

    Task<Either<PreRegistrationFetchingError, PreRegistrationId>> UpdatePreRegistrationByIdAsync(
        Guid id,
        bool isApproved
    );
}
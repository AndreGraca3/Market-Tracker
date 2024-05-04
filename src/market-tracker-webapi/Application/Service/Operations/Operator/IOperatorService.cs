using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.Operator;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Operator;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.PreRegister;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Operator;

using Operator = Domain.Operator;

public interface IOperatorService
{
    Task<Either<IServiceError, PaginatedResult<OperatorItem>>> GetOperatorsAsync(
        bool? isApproved,
        int skip,
        int take
    );

    Task<Either<UserFetchingError, OperatorInfo>> GetOperatorByIdAsync(Guid id);

    Task<Either<PreRegistrationFetchingError, GuidOutputModel>> CreateOperatorAsync(Guid code, string password);

    Task<Either<UserFetchingError, Operator>> UpdateOperatorAsync(
        Guid id,
        int phoneNumber
    );

    Task<Either<UserFetchingError, GuidOutputModel>> DeleteOperatorAsync(Guid id);
}
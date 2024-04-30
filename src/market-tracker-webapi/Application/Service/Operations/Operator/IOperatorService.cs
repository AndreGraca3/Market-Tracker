using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Operator;
using market_tracker_webapi.Application.Repository.Dto.Operator;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Operator;

using Operator = Domain.Operator;

public interface IOperatorService
{
    Task<Either<UserFetchingError, OperatorInfo>> GetOperatorAsync(Guid id);

    Task<Either<UserCreationError, GuidOutputModel>> CreateOperatorAsync(int code);

    Task<Either<UserFetchingError, Operator>> UpdateClientAsync(
        Guid id,
        int phoneNumber
    );

    Task<Either<UserFetchingError, OperatorOutputModel>> DeleteClientAsync(Guid id);
}
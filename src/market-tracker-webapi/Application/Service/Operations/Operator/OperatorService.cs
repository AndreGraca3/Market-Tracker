using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Operator;
using market_tracker_webapi.Application.Repository.Dto.Operator;
using market_tracker_webapi.Application.Repository.Operations.Operator;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Operator;

using Operator = Domain.Operator;

public class OperatorService(
    IUserRepository userRepository,
    IOperatorRepository operatorRepository,
    ITransactionManager transactionManager
) : IOperatorService
{
    public async Task<Either<UserFetchingError, OperatorInfo>> GetOperatorAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var operatorInfo = await operatorRepository.GetOperatorByIdAsync(id);
            if (operatorInfo is null)
            {
                return EitherExtensions.Failure<UserFetchingError, OperatorInfo>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<UserFetchingError, OperatorInfo>(
                operatorInfo
            );
        });
    }

    public async Task<Either<UserCreationError, GuidOutputModel>> CreateOperatorAsync(int code)
    {
        throw new NotImplementedException("Not yet implemented");
        //return await transactionManager.ExecuteAsync(async () =>
        //{
        //    
        //    
        //});
    }

    public async Task<Either<UserFetchingError, Operator>> UpdateClientAsync(Guid id, int phoneNumber)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var user = await userRepository.GetUserByIdAsync(id);

            if (user is null)
            {
                return EitherExtensions.Failure<UserFetchingError, Operator>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            if (await operatorRepository.GetOperatorByIdAsync(id) is not null)
            {
                await operatorRepository.UpdateOperatorAsync(
                    id,
                    phoneNumber
                );
            }
            else
            {
                await operatorRepository.CreateOperatorAsync(id, phoneNumber);
            }

            return EitherExtensions.Success<UserFetchingError, Operator>(
                new Operator(id, phoneNumber)
            );
        });
    }

    public async Task<Either<UserFetchingError, OperatorOutputModel>> DeleteClientAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var user = await userRepository.DeleteUserAsync(id);
            if (user is null)
            {
                return EitherExtensions.Failure<UserFetchingError, OperatorOutputModel>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            var oper = await operatorRepository.DeleteOperatorAsync(id);

            return EitherExtensions.Success<UserFetchingError, OperatorOutputModel>(
                new OperatorOutputModel(
                    user.Id,
                    user.Username,
                    user.Name,
                    user.Email,
                    user.CreatedAt,
                    oper?.PhoneNumber
                )
            );
        });
    }
}
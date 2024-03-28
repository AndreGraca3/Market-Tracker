using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.User;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.User
{
    public class UserService(
        IUserRepository userRepository,
        TransactionManager transactionManager
    ) : IUserService
    {
        public async Task<Either<UserFetchingError, UserOutputModel>> GetUserAsync(Guid id)
        {
            return await transactionManager.ExecuteAsync(async () =>
            {
                var user = await userRepository.GetUserByIdAsync(id);
                if (user is null)
                {
                    return EitherExtensions.Failure<UserFetchingError, UserOutputModel>(
                        new UserFetchingError.UserByIdNotFound(id)
                    );
                }

                return EitherExtensions.Success<UserFetchingError, UserOutputModel>(
                    new UserOutputModel(user.Id, user.Username, user.Name)
                );
            });
        }

        public async Task<Either<UserCreationError, IdOutputModel>> CreateUserAsync(
            string username,
            string name,
            string email,
            string password
        )
        {
            return await transactionManager.ExecuteAsync(async () =>
            {
                if (await userRepository.GetUserByEmail(email) is not null)
                {
                    return EitherExtensions.Failure<UserCreationError, IdOutputModel>(
                        new UserCreationError.EmailAlreadyInUse(email)
                    );
                }

                var userId = await userRepository.CreateUserAsync(
                    username, name, email, password
                );

                return EitherExtensions.Success<UserCreationError, IdOutputModel>(
                    new IdOutputModel(userId)
                );
            });
        }

        public async Task<Either<UserFetchingError, UserOutputModel>> UpdateUserAsync(Guid id, string? name,
            string? username)
        {
            return await transactionManager.ExecuteAsync(async () =>
            {
                var user = await userRepository.UpdateUserAsync(id, name, username);

                if (user is null)
                {
                    return EitherExtensions.Failure<UserFetchingError, UserOutputModel>(
                        new UserFetchingError.UserByIdNotFound(id)
                    );
                }

                return EitherExtensions.Success<UserFetchingError, UserOutputModel>(
                    new UserOutputModel(
                        id, user.Username, user.Name
                    )
                );
            });
        }

        public async Task<Either<UserFetchingError, UserOutputModel>> DeleteUserAsync(Guid id)
        {
            return await transactionManager.ExecuteAsync(async () =>
            {
                var user = await userRepository.DeleteUserAsync(id);
                if (user is null)
                {
                    return EitherExtensions.Failure<UserFetchingError, UserOutputModel>(
                        new UserFetchingError.UserByIdNotFound(id)
                    );
                }

                return EitherExtensions.Success<UserFetchingError, UserOutputModel>(
                    new UserOutputModel(
                        id,
                        user.Username,
                        user.Name
                    )
                );
            });
        }
    }
}
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.User;
using market_tracker_webapi.Application.Repository.Operations.Token;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.User
{
    public class UserService(
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        TransactionManager transactionManager
    ) : IUserService
    {
        public async Task<UsersOutputModel> GetUsersAsync(string? username, Pagination pagination)
        {
            return await transactionManager.ExecuteAsync(async () =>
            {
                var users = (await userRepository.GetUsersAsync(username, pagination.Skip, pagination.Limit)).Select(
                    it =>
                        new UserOutputModel(it.Id, it.Username, it.Name, it.CreatedAt)
                ).ToArray();

                return new UsersOutputModel(users, users.Length);
            });
        }

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
                    new UserOutputModel(user.Id, user.Username, user.Name, user.CreatedAt)
                );
            });
        }

        // Helper function, does not return Either
        public async Task<AuthenticatedUser?> GetUserByToken(Guid tokenValue)
        {
            return await transactionManager.ExecuteAsync(async () =>
            {
                var token = await tokenRepository.GetTokenByTokenValueAsync(tokenValue);

                if (token is null || token.ExpiresAt <= DateTime.Now)
                {
                    return null;
                }

                var user = await userRepository.GetUserByIdAsync(token.UserId);

                return new AuthenticatedUser(user!, token);
            });
        }

        public async Task<Either<UserCreationError, UserCreationOutputModel>> CreateUserAsync(
            string username,
            string name,
            string email,
            string password,
            int? code
        )
        {
            return await transactionManager.ExecuteAsync(async () =>
            {
                if (await userRepository.GetUserByEmailAsync(email) is not null)
                {
                    return EitherExtensions.Failure<UserCreationError, UserCreationOutputModel>(
                        new UserCreationError.EmailAlreadyInUse(email)
                    );
                }

                // if(!code?.isValid) { return Failure<UserCreationError, > }

                var userId = await userRepository.CreateUserAsync(
                    username, name, email, password
                );

                return EitherExtensions.Success<UserCreationError, UserCreationOutputModel>(
                    new UserCreationOutputModel(userId, UserCreationOutputModel.Client)
                );
            });
        }

        public async Task<Either<UserFetchingError, UserOutputModel>> UpdateUserAsync(Guid id, string? name,
            string? username)
        {
            return await transactionManager.ExecuteAsync(async () =>
            {
                var user = await userRepository.UpdateUserAsync(id, name == "" ? null : name,
                    username == "" ? null : username);

                if (user is null)
                {
                    return EitherExtensions.Failure<UserFetchingError, UserOutputModel>(
                        new UserFetchingError.UserByIdNotFound(id)
                    );
                }

                return EitherExtensions.Success<UserFetchingError, UserOutputModel>(
                    new UserOutputModel(
                        id, user.Username, user.Name, user.CreatedAt
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
                        user.Name,
                        user.CreatedAt
                    )
                );
            });
        }
    }
}
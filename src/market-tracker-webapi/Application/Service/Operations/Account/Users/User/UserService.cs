using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.User;
using market_tracker_webapi.Application.Repository.Dto.User;
using market_tracker_webapi.Application.Repository.Operations.Account.Credential;
using market_tracker_webapi.Application.Repository.Operations.Account.Token;
using market_tracker_webapi.Application.Repository.Operations.Account.Users.User;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.User;

public class UserService(
    IUserRepository userRepository,
    ITokenRepository tokenRepository,
    IAccountRepository accountRepository,
    ITransactionManager transactionManager
) : IUserService
{
    public async Task<Either<IServiceError, PaginatedResult<UserItem>>> GetUsersAsync(
        string? role,
        int skip,
        int take
    )
    {
        return EitherExtensions.Success<IServiceError, PaginatedResult<UserItem>>(
            await userRepository.GetUsersAsync(role, skip, take)
        );
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
                new UserOutputModel(user.Id, user.Name, user.CreatedAt)
            );
        });
    }

    // Helper function, does not return Either
    public async Task<AuthenticatedUser?> GetUserByToken(Guid tokenValue)
    {
        var token = await tokenRepository.GetTokenByTokenValueAsync(tokenValue);

        if (token is null || token.ExpiresAt <= DateTime.Now)
        {
            return null;
        }

        var user = await userRepository.GetUserByIdAsync(token.UserId);

        return new AuthenticatedUser(user!, token);
    }

    public async Task<Either<UserCreationError, UserCreationOutputModel>> CreateUserAsync(
        string name,
        string email,
        string password,
        string role
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await userRepository.GetUserByEmailAsync(email) is not null)
            {
                return EitherExtensions.Failure<UserCreationError, UserCreationOutputModel>(
                    new UserCreationError.CredentialAlreadyInUse(email, nameof(email))
                );
            }

            var userId = await userRepository.CreateUserAsync(name, email, role);
            await accountRepository.CreatePasswordAsync(userId, password);

            return EitherExtensions.Success<UserCreationError, UserCreationOutputModel>(
                new UserCreationOutputModel(userId, UserCreationOutputModel.Client)
            );
        });
    }

    public async Task<Either<UserFetchingError, UserOutputModel>> UpdateUserAsync(
        Guid id,
        string? name
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var user = await userRepository.UpdateUserAsync(
                id,
                name == "" ? null : name
            );

            if (user is null)
            {
                return EitherExtensions.Failure<UserFetchingError, UserOutputModel>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<UserFetchingError, UserOutputModel>(
                new UserOutputModel(id, user.Name, user.CreatedAt)
            );
        });
    }

    public async Task<Either<UserFetchingError, GuidOutputModel>> DeleteUserAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var user = await userRepository.DeleteUserAsync(id);
            if (user is null)
            {
                return EitherExtensions.Failure<UserFetchingError, GuidOutputModel>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<UserFetchingError, GuidOutputModel>(
                new GuidOutputModel(id)
            );
        });
    }
}
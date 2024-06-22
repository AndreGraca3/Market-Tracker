using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Repository.Account.Credential;
using market_tracker_webapi.Application.Repository.Account.Token;
using market_tracker_webapi.Application.Repository.Account.Users.User;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Transaction;
using Microsoft.IdentityModel.Tokens;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.User;

using User = Domain.Schemas.Account.Users.User;

public class UserService(
    IUserRepository userRepository,
    ITokenRepository tokenRepository,
    IAccountRepository accountRepository,
    ITransactionManager transactionManager
) : IUserService
{
    public async Task<PaginatedResult<UserItem>> GetUsersAsync(
        string? role,
        int skip,
        int take
    )
    {
        return await userRepository.GetUsersAsync(role, skip, take);
    }

    public async Task<User> GetUserAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await userRepository.GetUserByIdAsync(id) ??
            throw new MarketTrackerServiceException(new UserFetchingError.UserByIdNotFound(id))
        );
    }

    // Helper function, does not return Either
    public async Task<AuthenticatedUser?> GetUserByToken(Guid tokenValue)
    {
        var token = await tokenRepository.GetTokenByTokenValueAsync(tokenValue);

        if (token is null || token.ExpiresAt <= DateTime.UtcNow)
        {
            return null;
        }

        var user = await userRepository.GetUserByIdAsync(token.UserId);

        return new AuthenticatedUser(user!, token);
    }

    public async Task<UserId> CreateUserAsync(
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
                throw new MarketTrackerServiceException(
                    new UserCreationError.CredentialAlreadyInUse(email, nameof(email))
                );
            }

            var userId = (await userRepository.CreateUserAsync(name, email, role)).Value;
            await accountRepository.CreatePasswordAsync(userId, password);

            return new UserId(userId);
        });
    }

    public async Task<User> UpdateUserAsync(Guid id, string? name)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var user = await userRepository.UpdateUserAsync(
                id,
                name.IsNullOrEmpty() ? null : name
            );

            if (user is null)
            {
                throw new MarketTrackerServiceException(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            return user;
        });
    }

    public async Task<UserId> DeleteUserAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
            (await userRepository.DeleteUserAsync(id))?.Id ??
            throw new MarketTrackerServiceException(new UserFetchingError.UserByIdNotFound(id))
        );
    }
}
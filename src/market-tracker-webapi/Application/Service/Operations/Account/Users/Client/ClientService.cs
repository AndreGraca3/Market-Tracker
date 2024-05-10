using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Models.Account.Users;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Account.Credential;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Repository.Account.Users.User;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.Client;

using Client = Domain.Models.Account.Users.Client;

public class ClientService(
    IAccountRepository accountRepository,
    IClientRepository clientRepository,
    IUserRepository userRepository,
    ITransactionManager transactionManager
) : IClientService
{
    public async Task<Either<IServiceError, PaginatedResult<ClientItem>>> GetClientsAsync(string? username, int skip,
        int take)
    {
        return EitherExtensions.Success<IServiceError, PaginatedResult<ClientItem>>(
            await clientRepository.GetClientsAsync(username, skip, take)
        );
    }

    public async Task<Either<UserFetchingError, Client>> GetClientByIdAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var client = await clientRepository.GetClientByIdAsync(id);
            if (client is null)
            {
                return EitherExtensions.Failure<UserFetchingError, Client>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<UserFetchingError, Client>(
                client
            );
        });
    }

    public async Task<Either<UserCreationError, UserId>> CreateClientAsync(
        string username,
        string name,
        string email,
        string? password,
        string? avatarUrl = null
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await userRepository.GetUserByEmailAsync(email) is not null)
            {
                return EitherExtensions.Failure<UserCreationError, UserId>(
                    new UserCreationError.CredentialAlreadyInUse(email, nameof(email))
                );
            }

            if (await clientRepository.GetClientByUsernameAsync(username) is not null)
            {
                return EitherExtensions.Failure<UserCreationError, UserId>(
                    new UserCreationError.CredentialAlreadyInUse(username, nameof(username))
                );
            }

            var userId = (await userRepository.CreateUserAsync(name, email, Role.Client.ToString())).Value;
            await clientRepository.CreateClientAsync(userId, username, avatarUrl);
            if (password is not null) await accountRepository.CreatePasswordAsync(userId, password);

            return EitherExtensions.Success<UserCreationError, UserId>(
                new UserId(userId)
            );
        });
    }

    public async Task<Either<UserFetchingError, Client>> UpdateClientAsync(Guid id, string? name, string? username,
        string? avatarUrl)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var user = await userRepository.UpdateUserAsync(id, name);

            if (await clientRepository.GetClientByIdAsync(id) is not null)
            {
                await clientRepository.UpdateClientAsync(
                    id,
                    username,
                    avatarUrl
                );
            }

            return EitherExtensions.Success<UserFetchingError, Client>(
                new Client(
                    user!,
                    username,
                    avatarUrl
                )
            );
        });
    }

    public async Task<Either<UserFetchingError, UserId>> DeleteClientAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () => EitherExtensions.Success<UserFetchingError, UserId>(
            (await userRepository.DeleteUserAsync(id))!.Id
        ));
    }
}
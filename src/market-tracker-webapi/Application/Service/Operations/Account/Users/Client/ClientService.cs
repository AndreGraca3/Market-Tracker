using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Account.Credential;
using market_tracker_webapi.Application.Repository.Account.Users.Client;
using market_tracker_webapi.Application.Repository.Account.Users.User;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Transaction;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.Client;

using Client = Domain.Schemas.Account.Users.Client;

public class ClientService(
    IAccountRepository accountRepository,
    IClientRepository clientRepository,
    IUserRepository userRepository,
    ITransactionManager transactionManager
) : IClientService
{
    public async Task<PaginatedResult<ClientItem>> GetClientsAsync(string? username, int skip,
        int take)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await clientRepository.GetClientsAsync(username, skip, take)
        );
    }

    public async Task<Client> GetClientByIdAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await clientRepository.GetClientByIdAsync(id) ?? throw new MarketTrackerServiceException(
                new UserFetchingError.UserByIdNotFound(id))
        );
    }

    public async Task<UserId> CreateClientAsync(
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
                throw new MarketTrackerServiceException(
                    new UserCreationError.CredentialAlreadyInUse(email, nameof(email))
                );
            }

            if (await clientRepository.GetClientByUsernameAsync(username) is not null)
            {
                throw new MarketTrackerServiceException(
                    new UserCreationError.CredentialAlreadyInUse(username, nameof(username))
                );
            }

            var userId = (await userRepository.CreateUserAsync(name, email, Role.Client.ToString()));
            await clientRepository.CreateClientAsync(userId.Value, username, avatarUrl);
            if (password is not null) await accountRepository.CreatePasswordAsync(userId.Value, password);

            return userId;
        });
    }

    public async Task<Client> UpdateClientAsync(Guid id, string? name, string? username,
        string? avatarUrl)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (username is not null && await userRepository.GetUserByUsernameAsync(username) is not null)
            {
                throw new MarketTrackerServiceException(
                    new UserCreationError.InvalidUsername(username)
                );
            }

            var user = await userRepository.UpdateUserAsync(id, name);

            var client = await clientRepository.UpdateClientAsync(
                id,
                username,
                avatarUrl
            ) ?? throw new MarketTrackerServiceException(
                new UserFetchingError.UserByIdNotFound(id)
            );

            return new Client(
                user!,
                client.Username,
                avatarUrl
            );
        });
    }

    public async Task<UserId> DeleteClientAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var deletedUser = await userRepository.DeleteUserAsync(id) ?? throw new MarketTrackerServiceException(
                new UserFetchingError.UserByIdNotFound(id)
            );

            return deletedUser.Id;
        });
    }
}
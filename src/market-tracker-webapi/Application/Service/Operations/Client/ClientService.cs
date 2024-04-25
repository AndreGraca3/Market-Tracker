using market_tracker_webapi.Application.Http.Models.Client;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Client;
using market_tracker_webapi.Application.Repository.Operations.Account;
using market_tracker_webapi.Application.Repository.Operations.Client;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Client;

using Client = Domain.Client;

public class ClientService(
    IAccountRepository accountRepository,
    IClientRepository clientRepository,
    IUserRepository userRepository,
    ITransactionManager transactionManager
) : IClientService
{
    private const string Role = "client";

    public async Task<PaginatedResult<ClientInfo>> GetClientsAsync(string? username, int skip, int take)
    {
        return await clientRepository.GetClientsAsync(username, skip, take);
    }

    public async Task<Either<UserFetchingError, ClientOutputModel>> GetClientAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var user = await userRepository.GetUserByIdAsync(id);
            var client = await clientRepository.GetClientByIdAsync(id);
            if (user is null)
            {
                return EitherExtensions.Failure<UserFetchingError, ClientOutputModel>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<UserFetchingError, ClientOutputModel>(
                new ClientOutputModel(user.Id, user.Username, user.Name, user.Email, user.CreatedAt, client?.AvatarUrl)
            );
        });
    }

    public async Task<Either<UserCreationError, ClientCreationOutputModel>> CreateClientAsync(
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
                return EitherExtensions.Failure<UserCreationError, ClientCreationOutputModel>(
                    new UserCreationError.EmailAlreadyInUse(email)
                );
            }

            var userId = await userRepository.CreateUserAsync(username, name, email, Role);
            if (avatarUrl is not null) await clientRepository.CreateClientAsync(userId, avatarUrl);
            if (password is not null) await accountRepository.CreatePasswordAsync(userId, password);

            return EitherExtensions.Success<UserCreationError, ClientCreationOutputModel>(
                new ClientCreationOutputModel(userId)
            );
        });
    }

    public async Task<Either<UserFetchingError, Client>> UpdateClientAsync(Guid id, string avatarUrl)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var client = await clientRepository.UpdateClientAsync(
                id,
                avatarUrl
            );

            if (client is null)
            {
                return EitherExtensions.Failure<UserFetchingError, Client>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<UserFetchingError, Client>(
                new Client(id, avatarUrl)
            );
        });
    }

    public async Task<Either<UserFetchingError, Client>> DeleteClientAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var client = await clientRepository.DeleteClientAsync(id);
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
}
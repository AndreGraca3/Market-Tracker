using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Client;
using market_tracker_webapi.Application.Pipeline.Authorization;
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
    public async Task<PaginatedResult<ClientInfo>> GetClientsAsync(string? username, int skip, int take)
    {
        return await clientRepository.GetClientsAsync(username, skip, take);
    }

    public async Task<Either<UserFetchingError, ClientInfo>> GetClientAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var client = await clientRepository.GetClientByIdAsync(id);
            if (client is null)
            {
                return EitherExtensions.Failure<UserFetchingError, ClientInfo>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<UserFetchingError, ClientInfo>(
                new ClientInfo(client.Id, client.Username, client.Name, client.Email, client.CreatedAt,
                    client.AvatarUrl)
            );
        });
    }

    public async Task<Either<UserCreationError, GuidOutputModel>> CreateClientAsync(
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
                return EitherExtensions.Failure<UserCreationError, GuidOutputModel>(
                    new UserCreationError.EmailAlreadyInUse(email)
                );
            }

            var userId = await userRepository.CreateUserAsync(username, name, email, Role.Client.ToString());
            if (avatarUrl is not null) await clientRepository.CreateClientAsync(userId, avatarUrl);
            if (password is not null) await accountRepository.CreatePasswordAsync(userId, password);

            return EitherExtensions.Success<UserCreationError, GuidOutputModel>(
                new GuidOutputModel(userId)
            );
        });
    }

    public async Task<Either<UserFetchingError, ClientInfo>> UpdateClientAsync(Guid id, string avatarUrl)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var user = await userRepository.GetUserByIdAsync(id);

            if (user is null)
            {
                return EitherExtensions.Failure<UserFetchingError, ClientInfo>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            if (await clientRepository.GetClientByIdAsync(id) is not null)
            {
                await clientRepository.UpdateClientAsync(
                    id,
                    avatarUrl
                );
            }
            else
            {
                await clientRepository.CreateClientAsync(id, avatarUrl);
            }

            return EitherExtensions.Success<UserFetchingError, ClientInfo>(
                new ClientInfo(
                    user,
                    avatarUrl
                )
            );
        });
    }
}
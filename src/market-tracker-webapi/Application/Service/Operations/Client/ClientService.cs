using market_tracker_webapi.Application.Http.Models.Client;
using market_tracker_webapi.Application.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Client;
using market_tracker_webapi.Application.Repository.Operations.Account;
using market_tracker_webapi.Application.Repository.Operations.Client;
using market_tracker_webapi.Application.Repository.Operations.User;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;
using Microsoft.OpenApi.Extensions;

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

    public async Task<Either<UserFetchingError, ClientOutputModel>> GetClientByIdAsync(Guid id)
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

            var userId = await userRepository.CreateUserAsync(username, name, email, Role.Client.GetDisplayName());
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
            var user = await userRepository.GetUserByIdAsync(id);

            if (user is null)
            {
                return EitherExtensions.Failure<UserFetchingError, Client>(
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

            return EitherExtensions.Success<UserFetchingError, Client>(
                new Client(id, avatarUrl)
            );
        });
    }

    public async Task<Either<UserFetchingError, ClientOutputModel>> DeleteClientAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var user = await userRepository.DeleteUserAsync(id);
            if (user is null)
            {
                return EitherExtensions.Failure<UserFetchingError, ClientOutputModel>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            var client = await clientRepository.DeleteClientAsync(id);

            return EitherExtensions.Success<UserFetchingError, ClientOutputModel>(
                new ClientOutputModel(
                    user.Id,
                    user.Username,
                    user.Name,
                    user.Email,
                    user.CreatedAt,
                    client?.AvatarUrl
                )
            );
        });
    }

    public async Task<Either<IServiceError, bool>> RegisterPushNotificationsAsync(Guid id, string deviceId, string firebaseToken)
    {
        return  await transactionManager.ExecuteAsync(async () =>
        {
            await clientRepository.UpsertFirebaseTokenAsync(id, deviceId, firebaseToken);

            return EitherExtensions.Success<IServiceError, bool>(true);
        });
    }

    public Task<Either<IServiceError, bool>> DeRegisterPushNotificationsAsync(Guid id, string deviceId)
    {
        return transactionManager.ExecuteAsync(async () =>
        {
            await clientRepository.RemoveFirebaseTokenAsync(id, deviceId);

            return EitherExtensions.Success<IServiceError, bool>(true);
        });
    }
}
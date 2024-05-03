using market_tracker_webapi.Application.Http.Models;
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

namespace market_tracker_webapi.Application.Service.Operations.Client;

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
                client
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
                    new UserCreationError.CredentialAlreadyInUse(email, nameof(email))
                );
            }

            if (await clientRepository.GetClientByUsernameAsync(username) is not null)
            {
                return EitherExtensions.Failure<UserCreationError, GuidOutputModel>(
                    new UserCreationError.CredentialAlreadyInUse(username, nameof(username))
                );
            }

            var userId = await userRepository.CreateUserAsync(name, email, Role.Client.ToString());
            await clientRepository.CreateClientAsync(userId, username, avatarUrl);
            if (password is not null) await accountRepository.CreatePasswordAsync(userId, password);

            return EitherExtensions.Success<UserCreationError, GuidOutputModel>(
                new GuidOutputModel(userId)
            );
        });
    }

    public async Task<Either<UserFetchingError, ClientInfo>> UpdateClientAsync(Guid id, string? name, string? username,
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

            return EitherExtensions.Success<UserFetchingError, ClientInfo>(
                new ClientInfo(
                    user!,
                    username,
                    avatarUrl
                )
            );
        });
    }

    public async Task<Either<UserFetchingError, GuidOutputModel>> DeleteClientAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var deletedUser = await userRepository.DeleteUserAsync(id);

            return EitherExtensions.Success<UserFetchingError, GuidOutputModel>(
                new GuidOutputModel(deletedUser!.Id)
            );
        });
    }
}
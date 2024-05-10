﻿using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Models.Account.Users;

namespace market_tracker_webapi.Application.Repository.Account.Users.Client;

using Client = Domain.Models.Account.Users.Client;

public interface IClientRepository
{
    Task<PaginatedResult<ClientItem>> GetClientsAsync(string? username, int skip, int limit);

    Task<Client?> GetClientByIdAsync(Guid id);

    Task<Client?> GetClientByUsernameAsync(string username);

    Task<Guid> CreateClientAsync(Guid userId, string username, string? avatarUrl);

    Task<ClientItem?> UpdateClientAsync(Guid id, string? username, string? avatarUrl = null);
    
    Task<Client?> DeleteClientAsync(Guid id);
}
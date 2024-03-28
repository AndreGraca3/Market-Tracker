﻿using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.User;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.User;

public interface IUserService
{
    Task<Either<UserFetchingError, UserOutputModel>> GetUserAsync(Guid id);

    Task<Either<UserCreationError, IdOutputModel>> CreateUserAsync(
        string username,
        string name,
        string email,
        string password
    );

    Task<Either<UserFetchingError, UserOutputModel>> UpdateUserAsync(
        Guid id,
        string? name,
        string? username
    );

    Task<Either<UserFetchingError, UserOutputModel>> DeleteUserAsync(Guid id);
}
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.User;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Operations.Account.Users.User;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Account.Users
{
    [ApiController]
    public class UserController(IUserService userService, ILogger<UserController> logger)
        : ControllerBase
    {
        [HttpGet(Uris.Users.Base)]
        [Authorized([Role.Moderator])]
        public async Task<ActionResult<PaginatedResult<UserItem>>> GetUsersAsync(
            [FromQuery] PaginationInputs paginationInputs,
            [FromQuery] string? role
        )
        {
            return ResultHandler.Handle(
                await userService.GetUsersAsync(role, paginationInputs.Skip,
                    paginationInputs.ItemsPerPage),
                _ => new ServerProblem.InternalServerError().ToActionResult()
            );
        }

        [HttpGet(Uris.Users.UserById)]
        [Authorized([Role.Moderator])]
        public async Task<ActionResult<UserOutputModel>> GetUserAsync(Guid id)
        {
            logger.LogDebug($"Call {nameof(GetUserAsync)} with {id}");

            return ResultHandler.Handle(
                await userService.GetUserAsync(id),
                error =>
                {
                    return error switch
                    {
                        UserFetchingError.UserByIdNotFound idNotFoundError
                            => new UserProblem.UserByIdNotFound(idNotFoundError).ToActionResult(),
                        _ => new ServerProblem.InternalServerError().ToActionResult()
                    };
                }
            );
        }

        [HttpPost(Uris.Users.Base)]
        [Authorized([Role.Moderator])]
        public async Task<ActionResult<UserCreationOutputModel>> CreateUserAsync(
            [FromBody] UserCreationInputModel userInput
        )
        {
            logger.LogDebug(
                $"Call {nameof(CreateUserAsync)} with {userInput.Name}, {userInput.Email}, {userInput.Password}"
            );

            var res = await userService.CreateUserAsync(
                userInput.Name,
                userInput.Email,
                userInput.Password,
                userInput.Role
            );

            return ResultHandler.Handle(
                res,
                error =>
                {
                    return error switch
                    {
                        UserCreationError.CredentialAlreadyInUse emailAlreadyInUse
                            => new UserProblem.UserAlreadyExists(
                                emailAlreadyInUse
                            ).ToActionResult(),

                        UserCreationError.InvalidEmail invalidEmail
                            => new UserProblem.InvalidEmail(invalidEmail).ToActionResult(),

                        _ => new ServerProblem.InternalServerError().ToActionResult()
                    };
                },
                idOutputModel =>
                    Created(Uris.Users.BuildUserByIdUri(idOutputModel.Id), idOutputModel)
            );
        }

        [HttpPut(Uris.Users.UserById)]
        [Authorized([Role.Moderator])]
        public async Task<ActionResult<UserOutputModel>> UpdateUserAsync(
            Guid id,
            [FromBody] UserUpdateInputModel userInput
        )
        {
            logger.LogDebug($"Call {nameof(GetUserAsync)} with ");

            return ResultHandler.Handle(
                await userService.UpdateUserAsync(id, userInput.Name),
                error =>
                {
                    return error switch
                    {
                        UserFetchingError.UserByIdNotFound userByIdNotFound
                            => new UserProblem.UserByIdNotFound(userByIdNotFound).ToActionResult(),

                        _ => new ServerProblem.InternalServerError().ToActionResult()
                    };
                }
            );
        }

        [HttpDelete(Uris.Users.UserById)]
        [Authorized([Role.Moderator])]
        public async Task<ActionResult<GuidOutputModel>> DeleteUserAsync(Guid id)
        {
            logger.LogDebug($"Call {nameof(DeleteUserAsync)} with {id}");

            return ResultHandler.Handle(
                await userService.DeleteUserAsync(id),
                error =>
                {
                    return error switch
                    {
                        UserFetchingError.UserByIdNotFound userByIdNotFound
                            => new UserProblem.UserByIdNotFound(userByIdNotFound).ToActionResult(),

                        _ => new ServerProblem.InternalServerError().ToActionResult()
                    };
                }
            );
        }
    }
}
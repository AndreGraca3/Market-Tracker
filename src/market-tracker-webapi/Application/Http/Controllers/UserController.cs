using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.User;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Operations.User;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers
{
    [ApiController]
    [Route(Uris.Users.Base)]
    public class UserController(IUserService userService, ILogger<UserController> logger)
        : ControllerBase
    {
        [HttpGet]
        [Authorized(["client"])]
        public async Task<ActionResult<UsersOutputModel>> GetUsersAsync(
            [FromQuery] PaginationInputs paginationInputs,
            [FromQuery] string? username
        )
        {
            var user = HttpContext.Items[AuthenticationDetails.KeyUser] as AuthenticatedUser;
            Console.WriteLine($"Authenticated User in controller method : {user?.User.Name}");
            logger.LogDebug($"Call {nameof(GetUsersAsync)} with {username}");

            return Ok(await userService.GetUsersAsync(username, paginationInputs.Skip, paginationInputs.ItemsPerPage));
        }

        [HttpGet(Uris.Users.UserById)]
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
                            => new UserProblem.UserByIdNotFound(idNotFoundError).ToActionResult()
                    };
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult<UserCreationOutputModel>> CreateUserAsync(
            [FromBody] UserCreationInputModel userInput
        )
        {
            logger.LogDebug(
                $"Call {nameof(CreateUserAsync)} with {userInput.Username}, {userInput.Name}, {userInput.Email}, {userInput.Password}"
            );

            var res = await userService.CreateUserAsync(
                userInput.Username,
                userInput.Name,
                userInput.Email,
                userInput.Password
            );

            return ResultHandler.Handle(
                res,
                error =>
                {
                    return error switch
                    {
                        UserCreationError.EmailAlreadyInUse emailAlreadyInUse
                            => new UserProblem.UserAlreadyExists(
                                emailAlreadyInUse
                            ).ToActionResult(),

                        UserCreationError.InvalidEmail invalidEmail
                            => new UserProblem.InvalidEmail(invalidEmail).ToActionResult()
                    };
                },
                idOutputModel =>
                    Created(Uris.Users.BuildUserByIdUri(idOutputModel.Id), idOutputModel)
            );
        }

        [HttpPut(Uris.Users.UserById)]
        public async Task<ActionResult<UserOutputModel>> UpdateUserAsync(
            Guid id,
            [FromBody] UserUpdateInputModel userInput
        )
        {
            logger.LogDebug($"Call {nameof(GetUserAsync)} with ");

            return ResultHandler.Handle(
                await userService.UpdateUserAsync(id, userInput.Name, userInput.Username),
                error =>
                {
                    return error switch
                    {
                        UserFetchingError.UserByIdNotFound userByIdNotFound
                            => new UserProblem.UserByIdNotFound(userByIdNotFound).ToActionResult()
                    };
                }
            );
        }

        [HttpDelete(Uris.Users.UserById)]
        public async Task<ActionResult<UserOutputModel>> DeleteUserAsync(Guid id)
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
                    };
                },
                _ => NoContent()
            );
        }
    }
}
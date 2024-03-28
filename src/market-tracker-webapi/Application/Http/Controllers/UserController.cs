using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.User;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Operations.User;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController(IUserService userService, ILogger<UserController> logger)
        : ControllerBase
    {
        [HttpGet("{id:Guid}")]
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
                    };
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult<IdOutputModel>> CreateUserAsync(
            [FromBody] UserCreationInputModel userInput
        )
        {
            logger.LogDebug(
                $"Call {nameof(CreateUserAsync)} with {userInput.Username}, {userInput.Name}, {userInput.Email}, {userInput.Password}");

            var res = await userService.CreateUserAsync(userInput.Username, userInput.Name, userInput.Email,
                userInput.Password);

            return ResultHandler.Handle(
                res,
                error =>
                {
                    return error switch
                    {
                        UserCreationError.EmailAlreadyInUse emailAlreadyInUse => new UserProblem.UserAlreadyExists(
                            emailAlreadyInUse).ToActionResult(),

                        UserCreationError.InvalidEmail invalidEmail => new UserProblem.InvalidEmail(invalidEmail)
                            .ToActionResult()
                    };
                }
                //, _ => new NotFoundObjectResult()
            );
        }

        [HttpPut("{id:guid}")]
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
                        UserFetchingError.UserByIdNotFound userByIdNotFound => new UserProblem
                            .UserByIdNotFound(userByIdNotFound).ToActionResult()
                    };
                }
            );
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<UserOutputModel>> DeleteUserAsync(Guid id)
        {
            logger.LogDebug($"Call {nameof(DeleteUserAsync)} with {id}");

            return ResultHandler.Handle(
                await userService.DeleteUserAsync(id),
                error =>
                {
                    return error switch
                    {
                        UserFetchingError.UserByIdNotFound userByIdNotFound => new UserProblem.UserByIdNotFound(
                            userByIdNotFound).ToActionResult()
                    };
                }
            );
        }
    }
}
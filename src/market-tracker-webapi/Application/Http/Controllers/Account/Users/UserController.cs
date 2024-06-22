using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.User;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.Account.Users.User;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Account.Users;

[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet(Uris.Users.Base)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<PaginatedResult<UserItem>>> GetUsersAsync(
        [FromQuery] PaginationInputs paginationInputs,
        [FromQuery] string? role
    )
    {
        return await userService.GetUsersAsync(role, paginationInputs.Skip, paginationInputs.ItemsPerPage);
    }

    [HttpGet(Uris.Users.UserById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<UserOutputModel>> GetUserAsync(Guid id)
    {
        return (await userService.GetUserAsync(id)).ToOutputModel();
    }

    [HttpPost(Uris.Users.Base)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<UserCreationOutputModel>> CreateUserAsync(
        [FromBody] UserCreationInputModel userInput
    )
    {
        var userId = await userService.CreateUserAsync(
            userInput.Name,
            userInput.Email,
            userInput.Password,
            userInput.Role
        );

        return Created(Uris.Users.BuildUserByIdUri(userId.Value), userId);
    }

    [HttpPut(Uris.Users.UserById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<UserOutputModel>> UpdateUserAsync(Guid id,
        [FromBody] UserUpdateInputModel userInput)
    {
        return (await userService.UpdateUserAsync(id, userInput.Name)).ToOutputModel();
    }

    [HttpDelete(Uris.Users.UserById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult> DeleteUserAsync(Guid id)
    {
        await userService.DeleteUserAsync(id);
        return NoContent();
    }
}
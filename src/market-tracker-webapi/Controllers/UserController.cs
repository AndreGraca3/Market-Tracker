using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Repositories.User;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController(IUserRepository userRepository, ILogger<UserController> logger)
        : ControllerBase
    {
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<UserData>> GetUserAsync(Guid id)
        {
            logger.LogDebug($"Call {nameof(GetUserAsync)} with {id}");

            var user = await userRepository.GetUserAsync(id);
            return user is null ? NotFound("User not found!") : Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateUserAsync(
            [FromBody] string username,
            [FromBody] string name,
            [FromBody] string email,
            [FromBody] string password
        )
        {
            logger.LogDebug($"Call {nameof(CreateUserAsync)} with {username}, {name}, {email}, {password}");

            return Ok(await userRepository.CreateUserAsync(username, name, email, password));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<UserDetailsData?>> UpdateUserAsync(
            [FromBody] Guid id,
            [FromBody] string? name,
            [FromBody] string? username
        )
        {
            logger.LogDebug($"Call {nameof(GetUserAsync)} with ");

            var updatedUser = await userRepository.UpdateUserAsync(id, name, username);
            return updatedUser is null ? NotFound("User not found!") : Ok(updatedUser);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteUserAsync(Guid id)
        {
            logger.LogDebug($"Call {nameof(DeleteUserAsync)} with {id}");

            var deletedUserId = await userRepository.DeleteUserAsync(id);
            return deletedUserId is null ? NotFound("User not found!") : Ok(deletedUserId);
        }
    }
}
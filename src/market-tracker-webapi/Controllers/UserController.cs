using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Repository;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userQuery;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userQuery, ILogger<UserController> logger)
        {   
            _userQuery = userQuery;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserAsync(int id)
        {
            _logger.LogDebug($"Call {nameof(GetUserAsync)} with {id}");

            var user = await _userQuery.GetUser(id);
            return user is null ? NotFound("User not found!") : Ok(user);
        }
    }
}

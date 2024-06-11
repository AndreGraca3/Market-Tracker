using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Client;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.Account.Users.Client;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Account.Users;

[ApiController]
[Produces(Uris.JsonMediaType, Problems.Problem.MediaType)]
public class ClientController(IClientService clientService, IClientDeviceService clientDeviceService) : ControllerBase
{
    [HttpGet(Uris.Clients.Base)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<PaginatedResult<ClientItemOutputModel>>> GetClientsAsync(
        [FromQuery] PaginationInputs paginationInputs, [FromQuery] string? username)
    {
        var paginatedClients =
            await clientService.GetClientsAsync(username, paginationInputs.Skip, paginationInputs.ItemsPerPage);
        return paginatedClients.Select(c => c.ToOutputModel());
    }

    [HttpGet(Uris.Clients.ClientById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ClientOutputModel>> GetClientAsync(Guid id)
    {
        return (await clientService.GetClientByIdAsync(id)).ToOutputModel();
    }

    [HttpGet(Uris.Clients.Me)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ClientOutputModel>> GetAuthenticatedClientAsync()
    {
        var authUser = (HttpContext.Items[AuthenticationDetails.KeyUser] as AuthenticatedUser)!;
        return (await clientService.GetClientByIdAsync(authUser.User.Id.Value)).ToOutputModel();
    }

    [HttpPost(Uris.Clients.Base)]
    public async Task<ActionResult<UserId>> CreateClientAsync(
        [FromBody] ClientCreationInputModel clientInput)
    {
        var userId = await clientService.CreateClientAsync(
            clientInput.Username,
            clientInput.Name,
            clientInput.Email,
            clientInput.Password,
            clientInput.Avatar
        );

        return Created(Uris.Users.BuildUserByIdUri(userId.Value), userId);
    }

    [HttpPut(Uris.Clients.ClientById)]
    public async Task<ActionResult<ClientOutputModel>> UpdateUserAsync(
        Guid id,
        [FromBody] ClientUpdateInputModel clientInput
    )
    {
        return (await clientService.UpdateClientAsync(id, clientInput.Name, clientInput.Username, clientInput.Avatar))
            .ToOutputModel();
    }

    [HttpDelete(Uris.Clients.ClientById)]
    public async Task<ActionResult> DeleteClientAsync(Guid id)
    {
        await clientService.DeleteClientAsync(id);
        return NoContent();
    }

    [HttpPost(Uris.Clients.RegisterPushNotifications)]
    [Authorized([Role.Client])]
    public async Task<ActionResult> RegisterPushNotificationsAsync(
        [FromBody] PushNotificationRegistrationInputModel pushNotificationRegistrationInput)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        await clientDeviceService.UpsertNotificationDeviceAsync(
            authUser.User.Id.Value,
            pushNotificationRegistrationInput.DeviceId,
            pushNotificationRegistrationInput.FirebaseToken
        );

        return NoContent();
    }

    [HttpPost(Uris.Clients.DeRegisterPushNotifications)]
    [Authorized([Role.Client])]
    public async Task<ActionResult> DeRegisterPushNotificationsAsync(
        [FromBody] PushNotificationDeRegistrationInputModel pushNotificationRegistrationInput)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        await clientDeviceService.DeRegisterNotificationDeviceAsync(
            authUser.User.Id.Value,
            pushNotificationRegistrationInput.DeviceId
        );

        return NoContent();
    }
}
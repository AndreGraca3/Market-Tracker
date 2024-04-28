using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Client;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Operations.Client;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers;

[ApiController]
public class ClientController(IClientService clientService) : ControllerBase
{
    [HttpGet(Uris.Clients.Base)]
    public async Task<ActionResult<PaginatedResult<ClientOutputModel>>> GetClientsAsync(
        [FromQuery] PaginationInputs paginationInputs, [FromQuery] string? username)
    {
        return Ok(
            await clientService.GetClientsAsync(username, paginationInputs.Skip, paginationInputs.ItemsPerPage)
        );
    }

    [HttpGet(Uris.Clients.ClientById)]
    public async Task<ActionResult<ClientOutputModel>> GetClientAsync(Guid id)
    {
        return ResultHandler.Handle(
            await clientService.GetClientByIdAsync(id),
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

    [HttpPost(Uris.Clients.Base)]
    public async Task<ActionResult<ClientCreationOutputModel>> CreateClientAsync(
        [FromBody] ClientCreationInputModel clientInput
    )
    {
        var res = await clientService.CreateClientAsync(
            clientInput.Username,
            clientInput.Name,
            clientInput.Email,
            clientInput.Password,
            clientInput.Avatar
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

    [HttpPut(Uris.Clients.ClientById)]
    public async Task<ActionResult<Client>> UpdateUserAsync(
        Guid id,
        [FromBody] ClientUpdateInputModel clientInput
    )
    {
        return ResultHandler.Handle(
            await clientService.UpdateClientAsync(id, clientInput.Avatar),
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

    [HttpDelete(Uris.Clients.ClientById)]
    public async Task<ActionResult<ClientOutputModel>> DeleteUserAsync(Guid id)
    {
        return ResultHandler.Handle(
            await clientService.DeleteClientAsync(id),
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

    [HttpPost(Uris.Clients.RegisterPushNotifications)]
    public async Task<ActionResult<bool>> RegisterPushNotificationsAsync(
        [FromBody] PushNotificationRegistrationInputModel pushNotificationRegistrationInput)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        return ResultHandler.Handle(
            await clientService.RegisterPushNotificationsAsync(
                authUser.User.Id,
                pushNotificationRegistrationInput.DeviceId,
                pushNotificationRegistrationInput.FirebaseToken
            ),
            error => new ServerProblem.InternalServerError().ToActionResult(),
            _ => NoContent()
        );
    }
    
    [HttpPost(Uris.Clients.DeRegisterPushNotifications)]
    public async Task<ActionResult<bool>> DeRegisterPushNotificationsAsync(
        [FromBody] PushNotificationRegistrationInputModel pushNotificationRegistrationInput)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        return ResultHandler.Handle(
            await clientService.DeRegisterPushNotificationsAsync(
                authUser.User.Id,
                pushNotificationRegistrationInput.DeviceId
            ),
            error => new ServerProblem.InternalServerError().ToActionResult(),
            _ => NoContent()
        );
    }
}
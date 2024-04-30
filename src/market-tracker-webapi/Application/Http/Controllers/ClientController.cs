using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Client;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Client;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Operations.Client;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers
{
    [ApiController]
    [Route(Uris.Clients.Base)]
    public class ClientController(IClientService clientService) : ControllerBase
    {
        [HttpGet]
        [Authorized([Role.Client])]
        public async Task<ActionResult<PaginatedResult<ClientItem>>> GetClientsAsync(
            [FromQuery] PaginationInputs paginationInputs,
            [FromQuery] string? username
        )
        {
            return Ok(
                await clientService.GetClientsAsync(username, paginationInputs.Skip, paginationInputs.ItemsPerPage)
            );
        }

        [HttpGet(Uris.Clients.ClientById)]
        [Authorized([Role.Client])]
        public async Task<ActionResult<ClientInfo>> GetClientAsync(Guid id)
        {
            return ResultHandler.Handle(
                await clientService.GetClientAsync(id),
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

        [HttpGet(Uris.Clients.Me)]
        [Authorized([Role.Client])]
        public Task<ActionResult<User>> GetAuthenticatedClientAsync()
        {
            return Task.FromResult<ActionResult<User>>(
                Ok((HttpContext.Items[AuthenticationDetails.KeyUser] as AuthenticatedUser)!.User));
        }

        [HttpPost]
        public async Task<ActionResult<GuidOutputModel>> CreateClientAsync(
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
                        UserCreationError.CredentialAlreadyInUse credentialAlreadyInUse
                            => new UserProblem.UserAlreadyExists(
                                credentialAlreadyInUse 
                            ).ToActionResult(),

                        UserCreationError.InvalidEmail invalidEmail
                            => new UserProblem.InvalidEmail(invalidEmail).ToActionResult(),
                    };
                },
                idOutputModel =>
                    Created(Uris.Users.BuildUserByIdUri(idOutputModel.Id), idOutputModel)
            );
        }

        [HttpPut]
        [Authorized([Role.Client])]
        public async Task<ActionResult<ClientInfo>> UpdateUserAsync(
            [FromBody] ClientUpdateInputModel clientInput
        )
        {
            var user = HttpContext.Items[AuthenticationDetails.KeyUser] as AuthenticatedUser;

            return ResultHandler.Handle(
                await clientService.UpdateClientAsync(user!.User.Id, clientInput.Name, clientInput.Username,
                    clientInput.Avatar),
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

        [HttpDelete]
        [Authorized([Role.Client])]
        public async Task<ActionResult<GuidOutputModel>> DeleteUserAsync()
        {
            var user = HttpContext.Items[AuthenticationDetails.KeyUser] as AuthenticatedUser;

            return ResultHandler.Handle(
                await clientService.DeleteClientAsync(user!.User.Id),
                error =>
                {
                    return error switch
                    {
                        UserFetchingError.UserByIdNotFound userByIdNotFound
                            => new UserProblem.UserByIdNotFound(userByIdNotFound).ToActionResult()
                    };
                },
                _ =>
                {
                    HttpContext.Response.Cookies.Delete(AuthenticationDetails.NameAuthorizationCookie);
                    return new NoContentResult();
                }
            );
        }
    }
}
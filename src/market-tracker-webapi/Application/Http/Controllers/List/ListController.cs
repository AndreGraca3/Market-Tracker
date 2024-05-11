using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.List;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Operations.List;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.List;

[ApiController]
public class ListController(
    IListService listService
) : ControllerBase
{
    [HttpGet(Uris.Lists.Base)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<CollectionOutputModel<ShoppingList>>> GetListsAsync(
        string? listName,
        DateTime? createdAfter,
        bool? isArchived,
        bool isOwner
    )
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res =
            await listService.GetListsAsync(authUser.User.Id, isOwner, listName, createdAfter, isArchived);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }

    [HttpGet(Uris.Lists.ListById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ShoppingListOutputModel>> GetListByIdAsync(int listId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res = await listService.GetListByIdAsync(listId, authUser.User.Id);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ListFetchingError.ListByIdNotFound idNotFoundError
                        => new ListProblem.ListByIdNotFound(idNotFoundError).ToActionResult(),
                    ListFetchingError.UserDoesNotOwnList userDoNotOwnListError
                        => new ListProblem.UserDoesNotOwnList(userDoNotOwnListError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }

    [HttpPost(Uris.Lists.Base)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<IntIdOutputModel>> AddListAsync(
        [FromBody] ListCreationInputModel inputModel)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res = await listService.AddListAsync(authUser.User.Id, inputModel.ListName);

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    UserFetchingError.UserByIdNotFound idNotFoundError
                        => new UserProblem.UserByIdNotFound(idNotFoundError).ToActionResult(),
                    ListCreationError.ListNameAlreadyExists nameAlreadyExistsError
                        => new ListProblem.ListNameAlreadyExists(nameAlreadyExistsError).ToActionResult(),
                    ListFetchingError.UserDoesNotOwnList userDoNotOwnListError
                        => new ListProblem.UserDoesNotOwnList(userDoNotOwnListError).ToActionResult(),
                    ListCreationError.MaxListNumberReached maxListNumberReachedError
                        => new ListProblem.MaxListNumberReached(maxListNumberReachedError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            (outputModel) => Created(Uris.Lists.BuildListByIdUri(outputModel.Id), outputModel)
        );
    }

    [HttpPatch(Uris.Lists.ListById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ShoppingList>> UpdateListAsync(
        int listId,
        [FromBody] UpdateListInputModel inputModel
    )
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res =
            await listService.UpdateListAsync(listId, authUser.User.Id, inputModel.ListName, inputModel.IsArchived);

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ListFetchingError.ListByIdNotFound idNotFoundError
                        => new ListProblem.ListByIdNotFound(idNotFoundError).ToActionResult(),
                    ListUpdateError.ListIsArchived listIsArchivedError
                        => new ListProblem.ListIsArchived(listIsArchivedError).ToActionResult(),
                    ListFetchingError.UserDoesNotOwnList userDoNotOwnListError
                        => new ListProblem.UserDoesNotOwnList(userDoNotOwnListError).ToActionResult(),
                    ListCreationError.ListNameAlreadyExists nameAlreadyExistsError
                        => new ListProblem.ListNameAlreadyExists(nameAlreadyExistsError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }

    [HttpDelete(Uris.Lists.ListById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ShoppingList>> DeleteListAsync(int listId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res = await listService.DeleteListAsync(listId, authUser.User.Id);

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ListFetchingError.ListByIdNotFound idNotFoundError
                        => new ListProblem.ListByIdNotFound(idNotFoundError).ToActionResult(),
                    ListFetchingError.UserDoesNotOwnList userDoNotOwnListError
                        => new ListProblem.UserDoesNotOwnList(userDoNotOwnListError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            _ => NoContent()
        );
    }

    [HttpPost(Uris.Lists.ClientsByListId)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ListClient>> AddClientToListAsync(
        int listId,
        [FromBody] ListInviteInputModel inviteInputModel)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res
            = await listService.AddClientToListAsync(listId, authUser.User.Id, inviteInputModel.ClientId);

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    UserFetchingError.UserByIdNotFound idNotFoundError
                        => new UserProblem.UserByIdNotFound(idNotFoundError).ToActionResult(),
                    ListFetchingError.ListByIdNotFound idNotFoundError
                        => new ListProblem.ListByIdNotFound(idNotFoundError).ToActionResult(),
                    ListFetchingError.UserDoesNotOwnList userDoNotOwnListError
                        => new ListProblem.UserDoesNotOwnList(userDoNotOwnListError).ToActionResult(),
                    ListClientCreationError.ClientAlreadyInList clientAlreadyInListError
                        => new ListClientProblem.ClientAlreadyInList(clientAlreadyInListError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            _ => NoContent()
        );
    }

    [HttpDelete(Uris.Lists.ClientByListId)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ListClient>> RemoveClientFromListAsync(
        int listId,
        Guid clientId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res
            = await listService.RemoveClientFromListAsync(listId, authUser.User.Id, clientId);

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ListFetchingError.ListByIdNotFound idNotFoundError
                        => new ListProblem.ListByIdNotFound(idNotFoundError).ToActionResult(),
                    ListFetchingError.UserDoesNotOwnList userDoNotOwnListError
                        => new ListProblem.UserDoesNotOwnList(userDoNotOwnListError).ToActionResult(),
                    ListClientFetchingError.ClientInListNotFound clientInListNotFound
                        => new ListClientProblem.ClientInListNotFound(clientInListNotFound).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            _ => NoContent()
        );
    }
}
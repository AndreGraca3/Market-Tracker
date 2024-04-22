﻿using System.ComponentModel.DataAnnotations;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.List;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Repository.Dto.List;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Operations.List;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.List;

public class ListController(
    IListService listService
) : ControllerBase
{
    [HttpGet(Uris.Lists.Base)]
    public async Task<ActionResult<CollectionOutputModel>> GetListsAsync(
        [Required] Guid clientId,
        string? listName,
        DateTime? createdAfter,
        bool? isArchived,
        bool? isOwner
    )
    {
        var res = await listService.GetListsAsync(clientId, listName, createdAfter, isArchived, isOwner);
        return ResultHandler.Handle(
            res,
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

    [HttpGet(Uris.Lists.ListById)]
    public async Task<ActionResult<ListProduct>> GetListByIdAsync(int listId, [Required] Guid clientId)
    {
        var res = await listService.GetListByIdAsync(listId, clientId);
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
    public async Task<ActionResult<IntIdOutputModel>> AddListAsync(
        [FromBody] CreationListInputModel inputModel)
    {
        var res = await listService.AddListAsync(inputModel.ClientId, inputModel.ListName);
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
    public async Task<ActionResult<ListOfProducts>> UpdateListAsync(
        int listId,
        [Required] Guid clientId,
        [FromBody] UpdateListInputModel inputModel
    )
    {
        var res = await listService.UpdateListAsync(listId, clientId, inputModel.ListName, inputModel.IsArchived);
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
    public async Task<ActionResult<ListOfProducts>> DeleteListAsync(int listId, [Required] Guid clientId)
    {
        var res = await listService.DeleteListAsync(listId, clientId);
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
    
    [HttpPost(Uris.Lists.Clients)]
    public async Task<ActionResult<ListClient>> AddClientToListAsync(
        int listId, 
        [FromBody] CreateClientListInputModel inputModel,
        [Required] Guid clientId)
    {
        var res = await listService.AddClientToListAsync(listId, inputModel.ClientId, clientId);
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
            }
        );
    }
    
    [HttpDelete(Uris.Lists.Clients)]
    public async Task<ActionResult<ListClient>> RemoveClientFromListAsync(
        int listId, 
        [Required] Guid clientId)
    {
        var res = await listService.RemoveClientFromListAsync(listId, clientId);
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
                    ListClientFetchingError.ClientInListNotFound clientInListNotFound
                        => new ListClientProblem.ClientInListNotFound(clientInListNotFound).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            _ => NoContent()
        );
    }
}
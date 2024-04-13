using System.ComponentModel.DataAnnotations;
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
        DateTime? archivedAt,
        DateTime? createdAt)
    {
        var res = await listService.GetListsAsync(clientId, listName, archivedAt, createdAt);
        return ResultHandler.Handle(
            res,
            _ => new ServerProblem.InternalServerError().ToActionResult()
        );
    }
    
    [HttpGet(Uris.Lists.ListById)]
    public async Task<ActionResult<ListProduct>> GetListByIdAsync(int listId)
    {
        var res = await listService.GetListByIdAsync(listId);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ListFetchingError.ListByIdNotFound idNotFoundError
                        => new ListProblem.ListByIdNotFound(idNotFoundError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }
    
    [HttpPost(Uris.Lists.Base)]
    public async Task<ActionResult<IntIdOutputModel>> AddListAsync(CreationListInputModel inputModel)
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
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
        (outputModel) => Created(Uris.Lists.BuildListByIdUri(outputModel.Id),res)
        );
    }
    
    [HttpPut(Uris.Lists.ListById)]
    public async Task<ActionResult<ListOfProducts>> UpdateListAsync(
        int listId, 
        [Required] Guid clientId, 
        [FromBody] UpdateListInputModel inputModel)
    {
        var res = await listService.UpdateListAsync(listId, clientId, inputModel.ListName, inputModel.ArchivedAt);
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
                    UserPermissionsError.UserDoNotOwnList userDoNotOwnListError
                        => new UserProblem.UserDoNotOwnList(userDoNotOwnListError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }
    
    [HttpDelete(Uris.Lists.ListById)]
    public async Task<ActionResult<ListOfProducts>> DeleteListAsync(int listId)
    {
        var res = await listService.DeleteListAsync(listId);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ListFetchingError.ListByIdNotFound idNotFoundError
                        => new ListProblem.ListByIdNotFound(idNotFoundError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            _ => NoContent()
        );
    }
}
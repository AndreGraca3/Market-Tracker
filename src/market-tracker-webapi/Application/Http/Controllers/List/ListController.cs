using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
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
        Guid clientId, 
        string? listName, 
        DateTime? archivedAt)
    {
        var res = await listService.GetListsAsync(clientId, listName, archivedAt);
        return ResultHandler.Handle(
            res,
            _ => new ServerProblem.InternalServerError().ToActionResult()
        );
    }
    
    [HttpGet(Uris.Lists.ListById)]
    public async Task<ActionResult<ListProduct>> GetListByIdAsync(int id)
    {
        var res = await listService.GetListByIdAsync(id);
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
    public async Task<ActionResult<IntIdOutputModel>> AddListAsync(Guid clientId, string listName)
    {
        var res = await listService.AddListAsync(clientId, listName);
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
            }
        );
    }
    
    [HttpPut(Uris.Lists.ListById)]
    public async Task<ActionResult<ListOfProducts>> UpdateListAsync(
        int id, 
        Guid clientId, 
        string? listName, 
        DateTime? archivedAt)
    {
        var res = await listService.UpdateListAsync(id, clientId, listName, archivedAt);
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
    public async Task<ActionResult<ListOfProducts>> DeleteListAsync(int id)
    {
        var res = await listService.DeleteListAsync(id);
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
}
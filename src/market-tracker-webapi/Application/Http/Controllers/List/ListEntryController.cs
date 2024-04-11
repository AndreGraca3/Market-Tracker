using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Repository.Dto.List;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.ListEntry;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Operations.List;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.List;

public class ListEntryController(
    IListEntryService listEntryService
    ) : ControllerBase
{
    [HttpGet(Uris.Lists.ListEntriesByListId)]
    public async Task<ActionResult<CollectionOutputModel>> GetListEntriesAsync(
        int? listId, 
        string? productId, 
        int? storeId, 
        int? quantity)
    {
        var res = await listEntryService.GetListEntriesAsync(listId, productId, storeId, quantity);

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
    
    [HttpGet(Uris.Lists.ListEntriesByListIdAndProductId)]
    public async Task<ActionResult<ListEntryDetails>> GetListEntryByIdAsync(
        int listId, 
        string productId)
    {
        var res = await listEntryService.GetListEntryByIdAsync(listId, productId);

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ListEntryFetchingError.ListEntryByIdNotFound idNotFoundError
                        => new ListEntryProblem.ListEntryByIdNotFound(idNotFoundError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }
    
    [HttpPost(Uris.Lists.ListEntriesByListId)]
    public async Task<ActionResult<IntIdOutputModel>> AddListEntryAsync(
        int listId,
        string productId, 
        int storeId, 
        int quantity)
    {
        var res = await listEntryService.AddListEntryAsync(listId, productId, storeId, quantity);

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
                    ProductFetchingError.ProductByIdNotFound productNotFoundError
                        => new ProductProblem.ProductByIdNotFound(productNotFoundError).ToActionResult(),
                    StoreFetchingError.StoreByIdNotFound storeNotFoundError
                        => new StoreProblem.StoreByIdNotFound(storeNotFoundError).ToActionResult(),
                    ListEntryCreationError.ListEntryQuantityInvalid quantityInvalidError
                        => new ListEntryProblem.ListEntryQuantityInvalid(quantityInvalidError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }
    
    [HttpPut(Uris.Lists.ListEntriesByListIdAndProductId)]
    public async Task<ActionResult<ListEntry>> UpdateListEntryAsync(
        int listId, 
        string productId, 
        int storeId, 
        int quantity)
    {
        var res = await listEntryService.UpdateListEntryAsync(listId, productId, storeId, quantity);

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ListEntryFetchingError.ListEntryByIdNotFound idNotFoundError
                        => new ListEntryProblem.ListEntryByIdNotFound(idNotFoundError).ToActionResult(),
                    ListEntryCreationError.ListEntryQuantityInvalid quantityInvalidError
                        => new ListEntryProblem.ListEntryQuantityInvalid(quantityInvalidError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }
    
    [HttpDelete(Uris.Lists.ListEntriesByListIdAndProductId)]
    public async Task<ActionResult<ListEntry>> DeleteListEntryAsync(
        int listId, 
        string productId)
    {
        var res = await listEntryService.DeleteListEntryAsync(listId, productId);

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ListEntryFetchingError.ListEntryByIdNotFound idNotFoundError
                        => new ListEntryProblem.ListEntryByIdNotFound(idNotFoundError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }
    
    
}
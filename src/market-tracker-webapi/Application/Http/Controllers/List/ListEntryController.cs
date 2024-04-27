using System.ComponentModel.DataAnnotations;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.List;
using market_tracker_webapi.Application.Http.Models.ListEntry;
using market_tracker_webapi.Application.Http.Problem;
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
    [HttpPost(Uris.Lists.ProductsByListId)]
    public async Task<ActionResult<IntIdOutputModel>> AddListEntryAsync(
        int listId,
        [Required] Guid clientId,
        [FromBody] ListEntryCreationInputModel inputModel)
    {
        var res = await listEntryService.AddListEntryAsync(listId, clientId, inputModel.ProductId, inputModel.StoreId,
            inputModel.Quantity
        );

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
                    ProductFetchingError.ProductNotFoundInStore productNotFoundError
                        => new ProductProblem.ProductNotFoundInStore(productNotFoundError).ToActionResult(),
                    ProductFetchingError.OutOfStockInStore outOfStockError
                        => new ProductProblem.OutOfStockInStore(outOfStockError).ToActionResult(),
                    ProductFetchingError.ProductByIdNotFound productNotFoundError
                        => new ProductProblem.ProductByIdNotFound(productNotFoundError).ToActionResult(),
                    StoreFetchingError.StoreByIdNotFound storeNotFoundError
                        => new StoreProblem.StoreByIdNotFound(storeNotFoundError).ToActionResult(),
                    ListEntryCreationError.ListEntryQuantityInvalid quantityInvalidError
                        => new ListEntryProblem.ListEntryQuantityInvalid(quantityInvalidError).ToActionResult(),
                    ListFetchingError.UserDoesNotOwnList userDoesNotOwnListError
                        => new ListProblem.UserDoesNotOwnList(userDoesNotOwnListError).ToActionResult(),
                    ListEntryCreationError.ProductAlreadyInList productAlreadyInListError
                        => new ListEntryProblem.ProductAlreadyInList(productAlreadyInListError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            (outputModel) => Created(Uris.Lists.BuildListByIdUri(outputModel.Id), outputModel)
        );
    }

    [HttpPatch(Uris.Lists.ProductByListIdAndProductId)]
    public async Task<ActionResult<ListEntry>> UpdateListEntryAsync(
        int listId,
        [Required] Guid clientId,
        string productId,
        [FromBody] ListEntryUpdateInputModel inputModel)
    {
        var res = await listEntryService.UpdateListEntryAsync(listId, clientId, productId, inputModel.StoreId,
            inputModel.Quantity
        );

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ListEntryFetchingError.ListEntryByIdNotFound idNotFoundError
                        => new ListEntryProblem.ListEntryByIdNotFound(idNotFoundError).ToActionResult(),
                    ProductFetchingError.ProductNotFoundInStore productNotFoundError
                        => new ProductProblem.ProductNotFoundInStore(productNotFoundError).ToActionResult(),
                    ProductFetchingError.OutOfStockInStore outOfStockError
                        => new ProductProblem.OutOfStockInStore(outOfStockError).ToActionResult(),
                    ListEntryCreationError.ListEntryQuantityInvalid quantityInvalidError
                        => new ListEntryProblem.ListEntryQuantityInvalid(quantityInvalidError).ToActionResult(),
                    ProductFetchingError.ProductByIdNotFound productNotFoundError
                        => new ProductProblem.ProductByIdNotFound(productNotFoundError).ToActionResult(),
                    StoreFetchingError.StoreByIdNotFound storeNotFoundError
                        => new StoreProblem.StoreByIdNotFound(storeNotFoundError).ToActionResult(),
                    ListFetchingError.UserDoesNotOwnList userDoesNotOwnListError
                        => new ListProblem.UserDoesNotOwnList(userDoesNotOwnListError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }

    [HttpDelete(Uris.Lists.ProductByListIdAndProductId)]
    public async Task<ActionResult<ListEntry>> DeleteListEntryAsync(
        int listId,
        [Required] Guid clientId,
        string productId)
    {
        var res = await listEntryService.DeleteListEntryAsync(listId, clientId, productId);

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ListEntryFetchingError.ListEntryByIdNotFound idNotFoundError
                        => new ListEntryProblem.ListEntryByIdNotFound(idNotFoundError).ToActionResult(),
                    ListFetchingError.UserDoesNotOwnList userDoesNotOwnListError
                        => new ListProblem.UserDoesNotOwnList(userDoesNotOwnListError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            _ => NoContent()
        );
    }

    [HttpGet(Uris.Lists.ProductsByListId)]
    public async Task<ActionResult<ShoppingListEntriesOutputModel>> GetListEntriesAsync(
        int listId,
        [FromQuery] ShoppingListAlternativeType? alternativeType,
        [FromQuery] ListAlternativeFiltersInputModel filters,
        [Required] Guid clientId
    )
    {
        var res = await listEntryService.GetListEntriesAsync(listId, clientId, alternativeType,
            filters.CompanyIds,
            filters.StoreIds,
            filters.CityIds
        );

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ListFetchingError.ListByIdNotFound idNotFoundError
                        => new ListProblem.ListByIdNotFound(idNotFoundError).ToActionResult(),
                    ListFetchingError.UserDoesNotOwnList userDoesNotOwnListError
                        => new ListProblem.UserDoesNotOwnList(userDoesNotOwnListError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            (outputModel) => Ok(outputModel)
        );
    }
}
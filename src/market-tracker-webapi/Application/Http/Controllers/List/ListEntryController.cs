using System.ComponentModel.DataAnnotations;
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Domain.Models.List;
using market_tracker_webapi.Application.Http.Controllers.Account;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.Schemas.List.ListEntry;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.List;
using market_tracker_webapi.Application.Service.Errors.ListEntry;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Operations.List;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.List;

[ApiController]
public class ListEntryController(IListEntryService listEntryService) : ControllerBase
{
    [HttpPost(Uris.Lists.ProductsByListId)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<IntIdOutputModel>> AddListEntryAsync(
        int listId,
        [FromBody] ListEntryCreationInputModel inputModel)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res =
            await listEntryService.AddListEntryAsync(listId, authUser.User.Id, inputModel.ProductId,
                inputModel.StoreId,
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

    [HttpPatch(Uris.Lists.ProductByListId)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ListEntry>> UpdateListEntryAsync(
        int listId,
        string productId,
        [FromBody] ListEntryUpdateInputModel inputModel)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res
            = await listEntryService.UpdateListEntryAsync(listId, authUser.User.Id, productId, inputModel.StoreId,
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

    [HttpDelete(Uris.Lists.ProductByListId)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ListEntry>> DeleteListEntryAsync(
        int listId,
        string productId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res = await listEntryService.DeleteListEntryAsync(listId, authUser.User.Id, productId);

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
        [FromQuery] AlternativeListFiltersInputModel filters,
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
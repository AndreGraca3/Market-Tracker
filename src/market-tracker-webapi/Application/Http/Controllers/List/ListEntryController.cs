using System.ComponentModel.DataAnnotations;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Http.Models.Schemas.List.ListEntry;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.List;
using market_tracker_webapi.Application.Service.Results;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.List;

[ApiController]
public class ListEntryController(IListEntryService listEntryService) : ControllerBase
{
    [HttpPost(Uris.Lists.EntriesByListId)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ListEntryId>> AddListEntryAsync(
        string listId, [FromBody] ListEntryCreationInputModel inputModel)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var listEntryId =
            await listEntryService.AddListEntryAsync(listId, authUser.User.Id.Value, inputModel.ProductId,
                inputModel.StoreId,
                inputModel.Quantity
            );

        return Created(Uris.Lists.BuildListEntryByIdUri(listId, listEntryId.Value), listEntryId);
    }

    [HttpPatch(Uris.Lists.ListEntryEntryById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ListEntry>> UpdateListEntryAsync(
        string listId,
        string productId,
        [FromBody] ListEntryUpdateInputModel inputModel)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        return await listEntryService.UpdateListEntryAsync(
            listId, authUser.User.Id.Value, productId, inputModel.StoreId, inputModel.Quantity
        );
    }

    [HttpDelete(Uris.Lists.ListEntryEntryById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ListEntry>> DeleteListEntryAsync(
        string listId,
        string productId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        await listEntryService.DeleteListEntryAsync(listId, authUser.User.Id.Value, productId);
        return NoContent();
    }

    [HttpGet(Uris.Lists.EntriesByListId)]
    public async Task<ActionResult<ShoppingListEntriesResult>> GetListEntriesAsync(
        string listId,
        [FromQuery] ShoppingListAlternativeType? alternativeType,
        [FromQuery] AlternativeListFiltersInputModel filters,
        [Required] Guid clientId
    )
    {
        return await listEntryService.GetListEntriesAsync(listId, clientId, alternativeType,
            filters.CompanyIds,
            filters.StoreIds,
            filters.CityIds
        );
    }
}
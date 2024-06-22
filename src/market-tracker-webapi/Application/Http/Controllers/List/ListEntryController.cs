using market_tracker_webapi.Application.Domain.Filters.List;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Http.Models.Schemas.List.ListEntry;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.List;
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
    public async Task<ActionResult<ListEntryOutputModel>> UpdateListEntryAsync(
        string listId,
        string entryId,
        [FromBody] ListEntryUpdateInputModel inputModel)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        return (await listEntryService.UpdateListEntryAsync(
            listId, authUser.User.Id.Value, entryId, inputModel.StoreId, inputModel.Quantity
        )).ToOutputModel();
    }

    [HttpDelete(Uris.Lists.ListEntryEntryById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult> DeleteListEntryAsync(
        string listId,
        string entryId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        await listEntryService.DeleteListEntryAsync(listId, authUser.User.Id.Value, entryId);
        return NoContent();
    }

    [Authorized([Role.Client])]
    [HttpGet(Uris.Lists.EntriesByListId)]
    public async Task<ActionResult<ShoppingListEntriesResultOutputModel>> GetListEntriesAsync(
        string listId,
        [FromQuery] ShoppingListAlternativeType? alternativeType,
        [FromQuery] AlternativeListFiltersInputModel filters
    )
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        return (await listEntryService.GetListEntriesAsync(listId, authUser.User.Id.Value, alternativeType,
            filters.CompanyIds,
            filters.StoreIds,
            filters.CityIds
        )).ToOutputModel();
    }
}
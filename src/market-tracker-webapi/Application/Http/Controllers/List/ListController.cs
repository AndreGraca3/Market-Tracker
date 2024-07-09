using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.List;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.List;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.List;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.List;

[ApiController]
public class ListController(IListService listService) : ControllerBase
{
    [HttpGet(Uris.Lists.Base)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<CollectionOutputModel<ShoppingListOutputModel>>> GetListsAsync(
        string? listName,
        DateTime? createdAfter,
        bool? isArchived,
        bool? isOwner
    )
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var lists =
            await listService.GetListsAsync(authUser.User.Id.Value, isOwner, listName, createdAfter, isArchived);
        return lists.Select(l => l.ToOutputModel(authUser.User.Id.Value)).ToCollectionOutputModel();
    }

    [HttpGet(Uris.Lists.ListById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ShoppingListSocialOutputModel>> GetListByIdAsync(string listId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        return (await listService.GetListByIdAsync(listId, authUser.User.Id.Value)).ToOutputModel();
    }

    [HttpPost(Uris.Lists.Base)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ShoppingListId>> AddListAsync(
        [FromBody] ListCreationInputModel inputModel)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var shoppingListId = await listService.AddListAsync(authUser.User.Id.Value, inputModel.ListName);

        return Created(Uris.Lists.BuildListByIdUri(shoppingListId.Value), shoppingListId);
    }

    [HttpPatch(Uris.Lists.ListById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<ShoppingListItemOutputModel>> UpdateListAsync(
        string listId, [FromBody] UpdateListInputModel inputModel)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var updatedList = await listService.UpdateListAsync(listId, authUser.User.Id.Value, inputModel.ListName,
            inputModel.IsArchived);
        return updatedList.ToOutputModel(authUser.User.Id.Value);
    }

    [HttpDelete(Uris.Lists.ListById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult> DeleteListAsync(string listId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        await listService.DeleteListAsync(listId, authUser.User.Id.Value);
        return NoContent();
    }

    [HttpPost(Uris.Lists.ClientsByListId)]
    [Authorized([Role.Client])]
    public async Task<ActionResult> AddClientToListAsync(
        string listId, [FromBody] ListInviteInputModel inviteInputModel)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        await listService.AddClientToListAsync(listId, authUser.User.Id.Value, inviteInputModel.ClientId);

        return NoContent();
    }

    [HttpDelete(Uris.Lists.ClientByListId)]
    [Authorized([Role.Client])]
    public async Task<ActionResult> RemoveClientFromListAsync(string listId, Guid clientId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        await listService.RemoveClientFromListAsync(listId, authUser.User.Id.Value, clientId);

        return NoContent();
    }
}
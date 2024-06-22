using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.Market.Store;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Market.Store;

[ApiController]
public class StoreController(IStoreService storeService) : ControllerBase
{
    [HttpGet(Uris.Stores.Base)]
    public async Task<ActionResult<CollectionOutputModel<StoreOutputModel>>> GetStoresAsync(
        int? companyId, int? cityId, string? name)
    {
        var stores = await storeService.GetStoresAsync(companyId, cityId, name);
        return stores.Select(s => s.ToOutputModel()).ToCollectionOutputModel();
    }

    [HttpGet(Uris.Stores.StoreById)]
    public async Task<ActionResult<StoreOutputModel>> GetStoreByIdAsync(int id)
    {
        return (await storeService.GetStoreByIdAsync(id)).ToOutputModel();
    }

    [HttpPost(Uris.Stores.Base)]
    [Authorized([Role.Operator])]
    public async Task<ActionResult<StoreId>> AddStoreAsync(
        [FromBody] StoreCreationInputModel input)
    {
        var storeId = await storeService.AddStoreAsync(
            input.Name,
            input.Address,
            input.CityId,
            input.CompanyId,
            input.OperatorId
        );

        return Created(Uris.Stores.BuildStoreByIdUri(storeId.Value), storeId);
    }

    [HttpPut(Uris.Stores.StoreById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<StoreItemOutputModel>> UpdateStoreAsync(
        int id, [FromBody] StoreUpdateInputModel storeInput)
    {
        return (await storeService.UpdateStoreAsync(
            id,
            storeInput.Name,
            storeInput.Address,
            storeInput.CityId,
            storeInput.CompanyId
        )).ToOutputModel();
    }

    [HttpDelete(Uris.Stores.StoreById)]
    [Authorized([Role.Operator])]
    public async Task<ActionResult> DeleteStoreAsync(int id)
    {
        await storeService.DeleteStoreAsync(id);
        return NoContent();
    }
}
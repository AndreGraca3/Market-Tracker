using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Sales.Pricing;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Inventory.Product;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Price;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.Market.Alert;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Market;

[ApiController]
public class AlertController(IAlertService alertService) : ControllerBase
{
    [HttpGet(Uris.Alerts.Base)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<CollectionOutputModel<PriceAlertOutputModel>>> GetPriceAlertsByClientIdAsync(string? productId,
        int? storeId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var priceAlerts = await alertService.GetPriceAlertsByClientIdAsync(
            authUser.User.Id.Value, productId, storeId);

        return priceAlerts.Select(a => a.ToOutputModel()).ToCollectionOutputModel();
    }

    [HttpPost(Uris.Alerts.Base)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<PriceAlertId>> AddPriceAlertByProductIdAsync(
        [FromBody] PriceAlertCreationInputModel priceAlertInput
    )
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var priceAlertId = await alertService.AddPriceAlertAsync(
            authUser.User.Id.Value,
            priceAlertInput.ProductId,
            priceAlertInput.StoreId,
            priceAlertInput.PriceThreshold
        );

        return Created(Uris.Alerts.BuildAlertByIdUri(priceAlertId.Value), priceAlertId);
    }

    [HttpDelete(Uris.Alerts.AlertById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult> RemovePriceAlertByProductIdAsync(string alertId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        await alertService.RemovePriceAlertAsync(
            authUser.User.Id.Value,
            alertId
        );

        return NoContent();
    }
}
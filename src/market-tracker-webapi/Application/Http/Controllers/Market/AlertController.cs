using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.Product;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.Alert;
using market_tracker_webapi.Application.Service.Errors.Product;
using market_tracker_webapi.Application.Service.Operations.Alert;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Market;

[ApiController]
public class AlertController(IAlertService alertService) : ControllerBase
{
    [HttpGet(Uris.Alerts.Base)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<CollectionOutputModel<PriceAlert>>> GetPriceAlertsByClientIdAsync(string? productId,
        int? storeId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res = await alertService.GetPriceAlertsByClientIdAsync(
            authUser.User.Id, productId, storeId);

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ProductFetchingError.ProductByIdNotFound idNotFoundError
                        => new ProductProblem.ProductByIdNotFound(idNotFoundError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }

    [HttpPost(Uris.Alerts.Base)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<StringIdOutputModel>> AddPriceAlertByProductIdAsync(
        [FromBody] PriceAlertCreationInputModel priceAlertInput
    )
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res = await alertService.AddPriceAlertAsync(
            authUser.User.Id,
            priceAlertInput.ProductId,
            priceAlertInput.StoreId,
            priceAlertInput.PriceThreshold
        );

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    ProductFetchingError.ProductByIdNotFound idNotFoundError
                        => new ProductProblem.ProductByIdNotFound(idNotFoundError).ToActionResult(),
                    ProductFetchingError.OutOfStockInStore outOfStockError
                        => new ProductProblem.OutOfStockInStore(outOfStockError).ToActionResult(),
                    AlertCreationError.ProductAlreadyHasPriceAlertInStore productAlreadyHasPriceAlertError
                        => new AlertProblem.ProductAlreadyHasPriceAlert(productAlreadyHasPriceAlertError)
                            .ToActionResult(),
                    AlertCreationError.NoDeviceTokensFound noDeviceTokensError
                        => new AlertProblem.NoDeviceTokensFound(noDeviceTokensError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            outputModel => Created(Uris.Alerts.BuildAlertByIdUri(outputModel.Id), outputModel)
        );
    }

    [HttpDelete(Uris.Alerts.AlertById)]
    [Authorized([Role.Client])]
    public async Task<ActionResult<PriceAlert>> RemovePriceAlertByProductIdAsync(string alertId)
    {
        var authUser = (AuthenticatedUser)HttpContext.Items[AuthenticationDetails.KeyUser]!;
        var res = await alertService.RemovePriceAlertAsync(
            authUser.User.Id,
            alertId
        );

        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    AlertFetchingError.AlertByIdNotFound idNotFoundError
                        => new AlertProblem.AlertByIdNotFound(idNotFoundError).ToActionResult(),
                    AlertFetchingError.ClientDoesNotOwnAlert clientDoesNotOwnAlertError
                        => new AlertProblem.ClientDoesNotOwnAlert(clientDoesNotOwnAlertError).ToActionResult(),
                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            },
            _ => NoContent()
        );
    }
}
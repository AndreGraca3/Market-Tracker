using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Operator;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.PreRegister;
using market_tracker_webapi.Application.Service.Operations.Account.Auth.PreRegister;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Market.Store;

[ApiController]
public class StorePreRegisterController(IPreRegistrationService preRegistrationService) : ControllerBase
{
    [HttpGet(Uris.Stores.StoresPending)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<PaginatedResult<OperatorOutputModel>>> GetPendingOperatorsAsync(
        [FromQuery] bool? isValid,
        [FromQuery] PaginationInputs paginationInputs)
    {
        return ResultHandler.Handle(
            await preRegistrationService.GetPreRegistrationsAsync(isValid, paginationInputs.Skip,
                paginationInputs.ItemsPerPage),
            _ => new ServerProblem.InternalServerError().ToActionResult()
        );
    }

    [HttpGet(Uris.Stores.StoresPendingById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<PreRegisterInfo>> GetPendingOperatorByIdAsync(
        Guid id
    )
    {
        return ResultHandler.Handle(
            await preRegistrationService.GetPreRegistrationByIdAsync(id),
            error =>
            {
                return error switch
                {
                    PreRegistrationFetchingError.PreRegistrationByIdNotFound preRegisterFetchingError =>
                        new PreRegistrationProblem.PreRegistrationByIdNotFound(preRegisterFetchingError)
                            .ToActionResult()
                };
            }
        );
    }

    [HttpPost(Uris.Stores.StoresPreRegister)]
    public async Task<ActionResult<GuidOutputModel>> AddPreRegisterAsync(
        [FromBody] PreRegisterCreationInputModel form
    )
    {
        return ResultHandler.Handle(
            await preRegistrationService.AddPreRegistrationAsync(
                form.OperatorName,
                form.Email,
                form.PhoneNumber,
                form.StoreName,
                form.StoreAddress,
                form.CompanyName,
                form.CityName,
                form.Document
            ),
            error =>
            {
                return error switch
                {
                    PreRegistrationCreationError.EmailAlreadyInUse emailAlreadyInUse =>
                        new PreRegistrationProblem.OperatorAlreadyExists(emailAlreadyInUse).ToActionResult()
                };
            }
        );
    }

    [HttpPost(Uris.Stores.StoresPendingById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<GuidOutputModel>> UpdateStatePreRegistrationByIdAsync(
        Guid id,
        [FromBody] bool isApproved
    )
    {
        return ResultHandler.Handle(
            await preRegistrationService.UpdatePreRegistrationByIdAsync(id, isApproved),
            error =>
            {
                return error switch
                {
                    PreRegistrationFetchingError.PreRegistrationByIdNotFound idNotFound =>
                        new PreRegistrationProblem.PreRegistrationByIdNotFound(idNotFound).ToActionResult()
                };
            }
        );
    }
}
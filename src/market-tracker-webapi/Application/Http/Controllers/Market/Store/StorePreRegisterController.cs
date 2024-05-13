using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Operator;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
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
        var operators = await preRegistrationService.GetPreRegistrationsAsync(isValid, paginationInputs.Skip,
            paginationInputs.ItemsPerPage);
        return operators.Select(preRegister => preRegister.ToOutputModel());
    }

    [HttpGet(Uris.Stores.StoresPendingById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<PreRegisterOutputModel>> GetPendingOperatorByIdAsync(Guid id)
    {
        throw new Exception();
//        var preRegistration = await preRegistrationService.GetPreRegistrationByIdAsync(id);
//        return preRegistration.ToPreRegisterOutputModel(preRegistration); // TODO: discuss this
    }

    [HttpPost(Uris.Stores.StoresPreRegister)]
    public async Task<ActionResult<PreRegistrationCode>> AddPreRegisterAsync(
        [FromBody] PreRegisterCreationInputModel form)
    {
        return await preRegistrationService.AddPreRegistrationAsync(
            form.OperatorName,
            form.Email,
            form.PhoneNumber,
            form.StoreName,
            form.StoreAddress,
            form.CompanyName,
            form.CityName,
            form.Document
        );
    }

    [HttpPost(Uris.Stores.StoresPendingById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<PreRegistrationCode>> UpdateStatePreRegistrationByIdAsync(
        Guid id,
        [FromBody] bool isApproved
    )
    {
        return await preRegistrationService.UpdatePreRegistrationByIdAsync(id, isApproved);
    }
}
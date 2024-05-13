using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Operator;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.Account.Users.Operator;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Account.Users;

[ApiController]
public class OperatorController(IOperatorService operatorService) : ControllerBase
{
    [HttpGet(Uris.Operator.Base)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<PaginatedResult<OperatorOutputModel>>> GetOperatorsAsync(
        [FromQuery] PaginationInputs paginationInputs
    )
    {
        var paginatedResult = await operatorService.GetOperatorsAsync(paginationInputs.Skip,
            paginationInputs.ItemsPerPage);
        return paginatedResult.Select(operatorItem => operatorItem.ToOutputModel());
    }

    [HttpGet(Uris.Operator.OperatorById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<Operator>> GetOperatorAsync(Guid id)
    {
        return await operatorService.GetOperatorByIdAsync(id);
    }

    [HttpPost(Uris.Operator.Base)]
    public async Task<ActionResult<UserId>> CreateOperatorAsync(
        [FromQuery] Guid code,
        [FromBody] OperatorCreationInputModel operatorInput
    )
    {
        var userId = await operatorService.CreateOperatorAsync(code, operatorInput.Password);
        return Created(Uris.Operator.BuildOperatorByIdUri(userId.Value), userId);
    }


    [HttpPatch(Uris.Operator.OperatorById)]
    [Authorized([Role.Operator])]
    public async Task<ActionResult<Operator>> UpdateOperatorAsync(
        Guid id,
        [FromBody] OperatorUpdateInputModel operatorInput
    )
    {
        return await operatorService.UpdateOperatorAsync(id, operatorInput.NewPhoneNumber);
    }
}
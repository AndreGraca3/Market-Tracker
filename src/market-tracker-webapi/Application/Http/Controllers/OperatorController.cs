using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Operator;
using market_tracker_webapi.Application.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Operator;
using market_tracker_webapi.Application.Service.Operations.Operator;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers;

[ApiController]
[Route(Uris.Operator.Base)]
public class OperatorController(IOperatorService operatorService) : ControllerBase
{
    /*[HttpGet]
    [Authorized([Role.Operator])]
    public async Task<ActionResult<PaginatedResult<OperatorOutputModel>>> GetOperatorsAsync(
        [FromQuery] PaginationInputs paginationInputs,
        [FromQuery] string? username
    )
    {
        return Ok(await operatorService.GetOperatorsAsync(username, paginationInputs.Skip,
            paginationInputs.ItemsPerPage));
    }*/

    [HttpGet(Uris.Operator.OperatorById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<OperatorInfo>> GetOperatorAsync(
        Guid id
    )
    {
        throw new NotImplementedException("Not yet implemented");
    }

    [HttpPost]
    [Authorized([Role.Operator])]
    public async Task<ActionResult<GuidOutputModel>> CreateOperatorAsync(
        [FromQuery] int code
    )
    {
        throw new NotImplementedException("Not yet implemented");
    }


    [HttpPatch(Uris.Operator.OperatorById)]
    [Authorized([Role.Operator])]
    public async Task<ActionResult<OperatorOutputModel>> UpdateOperatorAsync(
        Guid id,
        [FromBody] OperatorUpdateInputModel operatorInput
    )
    {
        throw new NotImplementedException("Not yet implemented");

        //return ResultHandler.Handle(
        //    await operatorService.UpdateOperatorsAsync(id, operatorInput.NewPhoneNumber),
        //    error =>
        //    {
        //        return error switch
        //        {
        //        };
        //    }
        //);
    }
}
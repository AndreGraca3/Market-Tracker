using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Models.Account.Users;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Operator;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.PreRegister;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Operations.Account.Users.Operator;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Account.Users;

[ApiController]
public class OperatorController(IOperatorService operatorService) : ControllerBase
{
    [HttpGet(Uris.Operator.Base)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<PaginatedResult<OperatorOutputModel>>> GetOperatorsAsync(
        [FromQuery] PaginationInputs paginationInputs,
        [FromQuery] bool? isApproved
    )
    {
        return ResultHandler.Handle(
            await operatorService.GetOperatorsAsync(isApproved, paginationInputs.Skip,
                paginationInputs.ItemsPerPage),
            _ => new ServerProblem.InternalServerError().ToActionResult()
        );
    }

    [HttpGet(Uris.Operator.OperatorById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<Operator>> GetOperatorAsync(
        Guid id
    )
    {
        return ResultHandler.Handle(
            await operatorService.GetOperatorByIdAsync(id),
            error =>
            {
                return error switch
                {
                    UserFetchingError.UserByIdNotFound idNotFound =>
                        new UserProblem.UserByIdNotFound(idNotFound).ToActionResult(),

                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }

    [HttpPost(Uris.Operator.Base)]
    public async Task<ActionResult<GuidOutputModel>> CreateOperatorAsync(
        [FromQuery] Guid code,
        [FromBody] OperatorCreationInputModel operatorInput
    )
    {
        return ResultHandler.Handle(
            await operatorService.CreateOperatorAsync(code, operatorInput.Password),
            error =>
            {
                return error switch
                {
                    PreRegistrationFetchingError.PreRegistrationByIdNotFound idNotFound =>
                        new PreRegistrationProblem.PreRegistrationByIdNotFound(idNotFound).ToActionResult(),

                    PreRegistrationFetchingError.PreRegistrationNotValidated notValidated =>
                        new PreRegistrationProblem.PreRegistrationNotValidated(notValidated).ToActionResult(),

                    _ => new ServerProblem.InternalServerError().ToActionResult()
                };
            }
        );
    }


    [HttpPatch(Uris.Operator.OperatorById)]
    [Authorized([Role.Operator])]
    public async Task<ActionResult<Operator>> UpdateOperatorAsync(
        Guid id,
        [FromBody] OperatorUpdateInputModel operatorInput
    )
    {
        return ResultHandler.Handle(
            await operatorService.UpdateOperatorAsync(id, operatorInput.NewPhoneNumber),
            error =>
            {
                return error switch
                {
                    UserFetchingError.UserByIdNotFound idNotFound =>
                        new UserProblem.UserByIdNotFound(idNotFound).ToActionResult(),
                    
                    _ => new ServerProblem.InternalServerError().ToActionResult(),
                };
            }
        );
    }
}
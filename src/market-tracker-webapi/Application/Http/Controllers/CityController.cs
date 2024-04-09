using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.City;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Service.Operations.City;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers;

[ApiController]
public class CityController(ICityService cityService) : ControllerBase
{
    [HttpGet(Uris.Cities.Base)]
    public async Task<ActionResult<CollectionOutputModel>> GetCitiesAsync()
    {
        var res = await cityService.GetCitiesAsync();
        return ResultHandler.Handle(res, _ => new ServerProblem.InternalServerError().ToActionResult());
    }

    [HttpGet(Uris.Cities.CityById)]
    public async Task<ActionResult<Domain.City>> GetCityByIdAsync(int id)
    {
        var res = await cityService.GetCityByIdAsync(id);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    CityFetchingError.CityByIdNotFound idNotFoundError
                        => new CityProblem.CityByIdNotFound(idNotFoundError).ToActionResult(),
                    _
                        => new ServerProblem.InternalServerError(
                            nameof(CityController)
                        ).ToActionResult()
                };
            }
        );
    }

    [HttpPost(Uris.Cities.Base)]
    public async Task<ActionResult<IntIdOutputModel>> AddCityAsync(
        [FromBody] CityCreationInputModel cityInput
    )
    {
        var res = await cityService.AddCityAsync(cityInput.CityName);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    CityCreationError.CityNameAlreadyExists cityNameError
                        => new CityProblem.CityNameAlreadyExists(cityNameError).ToActionResult(),
                    _
                        => new ServerProblem.InternalServerError(
                            nameof(CityController)
                        ).ToActionResult()
                };
            },
            outputModel => Created(Uris.Cities.BuildCategoryByIdUri(outputModel.Id), outputModel)
        );
    }

    [HttpPut(Uris.Cities.CityById)]
    public async Task<ActionResult<Domain.City>> UpdateCityAsync(
        int id,
        [FromBody] CityUpdateInputModel cityInput
    )
    {
        var res = await cityService.UpdateCityAsync(id, cityInput.CityName);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    CityFetchingError.CityByIdNotFound idNotFoundError
                        => new CityProblem.CityByIdNotFound(idNotFoundError).ToActionResult(),
                    CityCreationError.CityNameAlreadyExists cityNameError
                        => new CityProblem.CityNameAlreadyExists(cityNameError).ToActionResult(),
                    _
                        => new ServerProblem.InternalServerError(
                            nameof(CityController)
                        ).ToActionResult()
                };
            }
        );
    }

    [HttpDelete(Uris.Cities.CityById)]
    public async Task<ActionResult<IntIdOutputModel>> DeleteCityAsync(int id)
    {
        var res = await cityService.DeleteCityAsync(id);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    CityFetchingError.CityByIdNotFound idNotFoundError
                        => new CityProblem.CityByIdNotFound(idNotFoundError).ToActionResult(),
                    _
                        => new ServerProblem.InternalServerError(
                            nameof(CityController)
                        ).ToActionResult()
                };
            }
        );
    }
}
using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Service.Errors.City;
using market_tracker_webapi.Application.Service.Operations.City;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers;

[ApiController]
public class CityController(ICityService cityService) : ControllerBase
{
    [HttpGet(Uris.Cities.Base)]
    public async Task<ActionResult<IEnumerable<CityDomain>>> GetCitiesAsync()
    {
        var cities = await cityService.GetCitiesAsync();
        return Ok(cities);
    }
    
    [HttpGet(Uris.Cities.CityById)]
    public async Task<ActionResult<CityDomain>> GetCityByIdAsync(int id)
    {
        var res = await cityService.GetCityByIdAsync(id);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    CityFetchingError.CityByIdNotFound idNotFoundError
                        => new CityProblem.CityByIdNotFound(
                            idNotFoundError
                        ).ToActionResult(),
                    _ => new ServerProblem.InternalServerError(nameof(CityController)).ToActionResult()
                };
            }
        );
    }
    
    [HttpPost(Uris.Cities.Base)]
    public async Task<ActionResult<IdOutputModel>> AddCityAsync([FromBody] CityCreationInputModel cityInput)
    {
        var res = await cityService.AddCityAsync(cityInput.CityName);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    CityCreationError.CityNameAlreadyExists _
                        => new CityProblem.CityNameAlreadyExists().ToActionResult(),
                    _ => new ServerProblem.InternalServerError(nameof(CityController)).ToActionResult()
                };
            }
        );
    }
    
    [HttpPut(Uris.Cities.CityById)]
    public async Task<ActionResult<CityDomain>> UpdateCityAsync(int id, [FromBody] CityUpdateInputModel cityInput)
    {
        var res = await cityService.UpdateCityAsync(id, cityInput.CityName);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    CityFetchingError.CityByIdNotFound idNotFoundError
                        => new CityProblem.CityByIdNotFound(
                            idNotFoundError
                        ).ToActionResult(),
                    CityCreationError.CityNameAlreadyExists _
                        => new CityProblem.CityNameAlreadyExists().ToActionResult(),
                    _ => new ServerProblem.InternalServerError(nameof(CityController)).ToActionResult()
                };
            }
        );
    }
    
    [HttpDelete(Uris.Cities.CityById)]
    public async Task<ActionResult<IdOutputModel>> DeleteCityAsync(int id)
    {
        var res = await cityService.DeleteCityAsync(id);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    CityFetchingError.CityByIdNotFound idNotFoundError
                        => new CityProblem.CityByIdNotFound(
                            idNotFoundError
                        ).ToActionResult(),
                    _ => new ServerProblem.InternalServerError(nameof(CityController)).ToActionResult()
                };
            }
        );
    }
}
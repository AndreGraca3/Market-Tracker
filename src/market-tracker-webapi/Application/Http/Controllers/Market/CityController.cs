using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.City;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.Market.City;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Market;

[ApiController]
[Produces(Uris.JsonMediaType, Uris.JsonProblemMediaType)]
public class CityController(ICityService cityService) : ControllerBase
{
    [HttpGet(Uris.Cities.Base)]
    public async Task<ActionResult<CollectionOutputModel<CityOutputModel>>> GetCitiesAsync()
    {
        var cities = await cityService.GetCitiesAsync();
        return cities.Select(c => c.ToOutputModel()).ToCollectionOutputModel();
    }

    [HttpGet(Uris.Cities.CityById)]
    public async Task<ActionResult<CityOutputModel>> GetCityByIdAsync(int id)
    {
        return (await cityService.GetCityByIdAsync(id)).ToOutputModel();
    }

    [HttpPost(Uris.Cities.Base)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<CityId>> AddCityAsync(
        [FromBody] CityCreationInputModel cityInput
    )
    {
        var cityId = await cityService.AddCityAsync(cityInput.CityName);
        return Created(Uris.Cities.BuildCityByIdUri(cityId.Value), cityId);
    }

    [HttpPut(Uris.Cities.CityById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<CityOutputModel>> UpdateCityAsync(
        int id,
        [FromBody] CityUpdateInputModel cityInput
    )
    {
        return (await cityService.UpdateCityAsync(id, cityInput.CityName)).ToOutputModel();
    }

    [HttpDelete(Uris.Cities.CityById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult> DeleteCityAsync(int id)
    {
        await cityService.DeleteCityAsync(id);
        return NoContent();
    }
}
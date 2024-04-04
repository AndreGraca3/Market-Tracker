using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Store;
using market_tracker_webapi.Application.Http.Problem;
using market_tracker_webapi.Application.Service.Errors.Store;
using market_tracker_webapi.Application.Service.Operations.Store;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers;

[ApiController]
public class StoreController(IStoreService storeService) : ControllerBase
{
    [HttpGet(Uris.Stores.Base)]
    public async Task<ActionResult<CollectionOutputModel>> GetStoresAsync()
    {
        var storesCollection = await storeService.GetStoresAsync();
        return Ok(storesCollection);
    }

    [HttpGet(Uris.Stores.StoreById)]
    public async Task<ActionResult<Domain.Store>> GetStoreByIdAsync(int id)
    {
        var res = await storeService.GetStoreByIdAsync(id);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    StoreFetchingError.StoreByIdNotFound idNotFoundError
                        => new StoreProblem.StoreByIdNotFound(idNotFoundError).ToActionResult(),
                    _
                        => new ServerProblem.InternalServerError(
                            nameof(StoreController)
                        ).ToActionResult()
                };
            }
        );
    }

    [HttpGet(Uris.Stores.StoresFromCompany)]
    public async Task<ActionResult<IEnumerable<Domain.Store>>> GetStoresFromCompanyAsync(
        int companyId
    )
    {
        var res = await storeService.GetStoresFromCompanyAsync(companyId);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    StoreFetchingError.StoreByCompanyIdNotFound companyIdNotFoundError
                        => new StoreProblem.StoreByCompanyIdNotFound(
                            companyIdNotFoundError
                        ).ToActionResult(),
                    _
                        => new ServerProblem.InternalServerError(
                            nameof(StoreController)
                        ).ToActionResult()
                };
            }
        );
    }

    [HttpGet(Uris.Stores.StoresByCityName)]
    public async Task<ActionResult<IEnumerable<Domain.Store>>> GetStoresByCityNameAsync(
        string cityName
    )
    {
        var res = await storeService.GetStoresByCityNameAsync(cityName);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    StoreFetchingError.StoreByCityNameNotFound cityNotFoundError
                        => new StoreProblem.StoreByCityNameNotFound(
                            cityNotFoundError
                        ).ToActionResult(),
                    _
                        => new ServerProblem.InternalServerError(
                            nameof(StoreController)
                        ).ToActionResult()
                };
            }
        );
    }

    [HttpPost(Uris.Stores.Base)]
    public async Task<ActionResult<IdOutputModel>> AddStoreAsync(
        [FromBody] StoreCreationInputModel model
    )
    {
        var res = await storeService.AddStoreAsync(
            model.Name,
            model.Address,
            model.CityId,
            model.CompanyId
        );
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    StoreCreationError.StoreAddressAlreadyExists addressAlreadyExistsError
                        => new StoreProblem.StoreAddressAlreadyExists(
                            addressAlreadyExistsError
                        ).ToActionResult(),
                    StoreCreationError.StoreNameAlreadyExists nameAlreadyExistsError
                        => new StoreProblem.StoreNameAlreadyExists(
                            nameAlreadyExistsError
                        ).ToActionResult(),
                    StoreFetchingError.StoreByCityIdNotFound cityIdNotFoundError
                        => new StoreProblem.StoreByCityIdNotFound(
                            cityIdNotFoundError
                        ).ToActionResult(),
                    StoreFetchingError.StoreByCompanyIdNotFound companyIdNotFoundError
                        => new StoreProblem.StoreByCompanyIdNotFound(
                            companyIdNotFoundError
                        ).ToActionResult(),
                    _
                        => new ServerProblem.InternalServerError(
                            nameof(StoreController)
                        ).ToActionResult()
                };
            }
        );
    }

    [HttpPut(Uris.Stores.StoreById)]
    public async Task<ActionResult<IdOutputModel>> UpdateStoreAsync(
        int id,
        [FromBody] StoreUpdateInputModel model
    )
    {
        var res = await storeService.UpdateStoreAsync(
            id,
            model.Name,
            model.Address,
            model.CityId,
            model.CompanyId
        );
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    StoreFetchingError.StoreByIdNotFound idNotFoundError
                        => new StoreProblem.StoreByIdNotFound(idNotFoundError).ToActionResult(),
                    StoreCreationError.StoreNameAlreadyExists nameAlreadyExistsError
                        => new StoreProblem.StoreNameAlreadyExists(
                            nameAlreadyExistsError
                        ).ToActionResult(),
                    StoreFetchingError.StoreByCityIdNotFound cityIdNotFoundError
                        => new StoreProblem.StoreByCityIdNotFound(
                            cityIdNotFoundError
                        ).ToActionResult(),
                    StoreFetchingError.StoreByCompanyIdNotFound companyIdNotFoundError
                        => new StoreProblem.StoreByCompanyIdNotFound(
                            companyIdNotFoundError
                        ).ToActionResult(),
                    _
                        => new ServerProblem.InternalServerError(
                            nameof(StoreController)
                        ).ToActionResult()
                };
            }
        );
    }

    [HttpDelete(Uris.Stores.StoreById)]
    public async Task<ActionResult<IdOutputModel>> DeleteStoreAsync(int id)
    {
        var res = await storeService.DeleteStoreAsync(id);
        return ResultHandler.Handle(
            res,
            error =>
            {
                return error switch
                {
                    StoreFetchingError.StoreByIdNotFound idNotFoundError
                        => new StoreProblem.StoreByIdNotFound(idNotFoundError).ToActionResult(),
                    _
                        => new ServerProblem.InternalServerError(
                            nameof(StoreController)
                        ).ToActionResult()
                };
            }
        );
    }
}

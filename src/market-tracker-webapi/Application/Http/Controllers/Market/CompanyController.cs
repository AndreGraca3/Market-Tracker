using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Company;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Service.Operations.Market.Company;
using Microsoft.AspNetCore.Mvc;

namespace market_tracker_webapi.Application.Http.Controllers.Market;

[ApiController]
[Produces(Uris.JsonMediaType, Problems.Problem.MediaType)]
public class CompanyController(ICompanyService companyService) : ControllerBase
{
    [HttpGet(Uris.Companies.Base)]
    public async Task<ActionResult<CollectionOutputModel<CompanyOutputModel>>> GetCompaniesAsync()
    {
        var companies = await companyService.GetCompaniesAsync();
        return companies.Select(c => c.ToOutputModel()).ToCollectionOutputModel();
    }

    [HttpGet(Uris.Companies.CompanyById)]
    public async Task<ActionResult<CompanyOutputModel>> GetCompanyByIdAsync(int id)
    {
        return (await companyService.GetCompanyByIdAsync(id)).ToOutputModel();
    }

    [HttpPost(Uris.Companies.Base)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<CompanyId>> AddCompanyAsync(
        [FromBody] CompanyCreationInputModel companyInput
    )
    {
        var companyId = await companyService.AddCompanyAsync(companyInput.CompanyName, companyInput.CompanyLogoUrl);
        return Created(Uris.Companies.BuildCompanyByIdUri(companyId.Value), companyId);
    }

    [HttpPut(Uris.Companies.CompanyById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult<CompanyOutputModel>> UpdateCompanyAsync(
        int id, [FromBody] CompanyUpdateInputModel companyInput)
    {
        return (await companyService.UpdateCompanyAsync(id, companyInput.CompanyName)).ToOutputModel();
    }

    [HttpDelete(Uris.Companies.CompanyById)]
    [Authorized([Role.Moderator])]
    public async Task<ActionResult> DeleteCompanyAsync(int id)
    {
        await companyService.DeleteCompanyAsync(id);
        return NoContent();
    }
}
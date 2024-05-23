using market_tracker_webapi.Application.Domain.Schemas.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.Market.Company;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Service.Transaction;

namespace market_tracker_webapi.Application.Service.Operations.Market.Company;

using Company = Domain.Schemas.Market.Retail.Shop.Company;

public class CompanyService(
    ICompanyRepository companyRepository,
    ITransactionManager transactionManager
) : ICompanyService
{
    public async Task<IEnumerable<Company>> GetCompaniesAsync()
    {
        return await transactionManager.ExecuteAsync(async () => await companyRepository.GetCompaniesAsync());
    }

    public async Task<Company> GetCompanyByIdAsync(int id)
    {
        return await companyRepository.GetCompanyByIdAsync(id) ?? throw new MarketTrackerServiceException(
            new CompanyFetchingError.CompanyByIdNotFound(id)
        );
    }

    public async Task<Company> GetCompanyByNameAsync(
        string companyName)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await companyRepository.GetCompanyByNameAsync(companyName) ??
            throw new MarketTrackerServiceException(new CompanyFetchingError.CompanyByNameNotFound(companyName))
        );
    }

    public async Task<CompanyId> AddCompanyAsync(string companyName, string companyLogoUrl)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await companyRepository.GetCompanyByNameAsync(companyName) is not null)
            {
                throw new MarketTrackerServiceException(
                    new CompanyCreationError.CompanyNameAlreadyExists(companyName)
                );
            }

            return await companyRepository.AddCompanyAsync(companyName, companyLogoUrl);
        });
    }

    public async Task<Company> UpdateCompanyAsync(int id, string companyName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await companyRepository.GetCompanyByNameAsync(companyName) is not null)
            {
                throw new MarketTrackerServiceException(
                    new CompanyCreationError.CompanyNameAlreadyExists(companyName)
                );
            }

            return await companyRepository.UpdateCompanyAsync(id, companyName) ??
                   throw new MarketTrackerServiceException(
                       new CompanyFetchingError.CompanyByIdNotFound(id)
                   );
        });
    }

    public async Task<CompanyId> DeleteCompanyAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
            (await companyRepository.DeleteCompanyAsync(id))?.Id ??
            throw new MarketTrackerServiceException(new CompanyFetchingError.CompanyByIdNotFound(id))
        );
    }
}
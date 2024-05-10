using market_tracker_webapi.Application.Domain.Models.Market.Retail.Shop;
using market_tracker_webapi.Application.Repository.Market.Company;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Company;

using Company = Domain.Models.Market.Retail.Shop.Company;

public class CompanyService(
    ICompanyRepository companyRepository,
    ITransactionManager transactionManager
) : ICompanyService
{
    public async Task<Either<IServiceError, IEnumerable<Company>>> GetCompaniesAsync()
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var companies = await companyRepository.GetCompaniesAsync();
            return EitherExtensions
                .Success<IServiceError, IEnumerable<Company>>(companies);
        });
    }

    public async Task<Either<CompanyFetchingError, Company>> GetCompanyByIdAsync(int id)
    {
        var company = await companyRepository.GetCompanyByIdAsync(id);
        return company is null
            ? EitherExtensions.Failure<CompanyFetchingError, Company>(
                new CompanyFetchingError.CompanyByIdNotFound(id)
            )
            : EitherExtensions.Success<CompanyFetchingError, Company>(company);
    }

    public async Task<Either<CompanyFetchingError, Company>> GetCompanyByNameAsync(
        string companyName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var company = await companyRepository.GetCompanyByNameAsync(companyName);
            return company is null
                ? EitherExtensions.Failure<CompanyFetchingError, Company>(
                    new CompanyFetchingError.CompanyByNameNotFound(companyName)
                )
                : EitherExtensions.Success<CompanyFetchingError, Company>(company);
        });
    }

    public async Task<Either<ICompanyError, CompanyId>> AddCompanyAsync(string companyName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await companyRepository.GetCompanyByNameAsync(companyName) is not null)
            {
                return EitherExtensions.Failure<ICompanyError, CompanyId>(
                    new CompanyCreationError.CompanyNameAlreadyExists(companyName)
                );
            }

            var companyId = await companyRepository.AddCompanyAsync(companyName);
            return EitherExtensions.Success<ICompanyError, CompanyId>(companyId);
        });
    }

    public async Task<Either<ICompanyError, Company>> UpdateCompanyAsync(
        int id,
        string companyName
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await companyRepository.GetCompanyByNameAsync(companyName) is not null)
            {
                return EitherExtensions.Failure<ICompanyError, Company>(
                    new CompanyCreationError.CompanyNameAlreadyExists(companyName)
                );
            }

            var company = await companyRepository.UpdateCompanyAsync(id, companyName);
            return company is null
                ? EitherExtensions.Failure<ICompanyError, Company>(
                    new CompanyFetchingError.CompanyByIdNotFound(id)
                )
                : EitherExtensions.Success<ICompanyError, Company>(
                    company
                );
        });
    }

    public async Task<Either<CompanyFetchingError, CompanyId>> DeleteCompanyAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var company = await companyRepository.DeleteCompanyAsync(id);
            return company is null
                ? EitherExtensions.Failure<CompanyFetchingError, CompanyId>(
                    new CompanyFetchingError.CompanyByIdNotFound(id)
                )
                : EitherExtensions.Success<CompanyFetchingError, CompanyId>(
                    company.Id
                );
        });
    }
}
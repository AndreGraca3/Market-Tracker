using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Repository.Market.Company;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Market.Company;

public class CompanyService(
    ICompanyRepository companyRepository,
    ITransactionManager transactionManager
) : ICompanyService
{
    public async Task<Either<IServiceError, CollectionOutputModel<Domain.Models.Market.Retail.Shop.Company>>> GetCompaniesAsync()
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var companies = await companyRepository.GetCompaniesAsync();
            return EitherExtensions.Success<IServiceError, CollectionOutputModel<Domain.Models.Market.Retail.Shop.Company>>(
                new CollectionOutputModel<Domain.Models.Market.Retail.Shop.Company>(companies)
            );
        });
    }

    public async Task<Either<CompanyFetchingError, Domain.Models.Market.Retail.Shop.Company>> GetCompanyByIdAsync(int id)
    {
        var company = await companyRepository.GetCompanyByIdAsync(id);
        return company is null
            ? EitherExtensions.Failure<CompanyFetchingError, Domain.Models.Market.Retail.Shop.Company>(
                new CompanyFetchingError.CompanyByIdNotFound(id)
            )
            : EitherExtensions.Success<CompanyFetchingError, Domain.Models.Market.Retail.Shop.Company>(company);
    }

    public async Task<Either<CompanyFetchingError, Domain.Models.Market.Retail.Shop.Company>> GetCompanyByNameAsync(string companyName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var company = await companyRepository.GetCompanyByNameAsync(companyName);
            return company is null
                ? EitherExtensions.Failure<CompanyFetchingError, Domain.Models.Market.Retail.Shop.Company>(
                    new CompanyFetchingError.CompanyByNameNotFound(companyName)
                )
                : EitherExtensions.Success<CompanyFetchingError, Domain.Models.Market.Retail.Shop.Company>(company);
        });
    }

    public async Task<Either<ICompanyError, IntIdOutputModel>> AddCompanyAsync(string companyName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await companyRepository.GetCompanyByNameAsync(companyName) is not null)
            {
                return EitherExtensions.Failure<ICompanyError, IntIdOutputModel>(
                    new CompanyCreationError.CompanyNameAlreadyExists(companyName)
                );
            }

            var companyId = await companyRepository.AddCompanyAsync(companyName);
            return EitherExtensions.Success<ICompanyError, IntIdOutputModel>(
                new IntIdOutputModel(companyId)
            );
        });
    }

    public async Task<Either<ICompanyError, Domain.Models.Market.Retail.Shop.Company>> UpdateCompanyAsync(
        int id,
        string companyName
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await companyRepository.GetCompanyByNameAsync(companyName) is not null)
            {
                return EitherExtensions.Failure<ICompanyError, Domain.Models.Market.Retail.Shop.Company>(
                    new CompanyCreationError.CompanyNameAlreadyExists(companyName)
                );
            }

            var company = await companyRepository.UpdateCompanyAsync(id, companyName);
            return company is null
                ? EitherExtensions.Failure<ICompanyError, Domain.Models.Market.Retail.Shop.Company>(
                    new CompanyFetchingError.CompanyByIdNotFound(id)
                )
                : EitherExtensions.Success<ICompanyError, Domain.Models.Market.Retail.Shop.Company>(
                    new Domain.Models.Market.Retail.Shop.Company
                    {
                        Id = company.Id,
                        Name = company.Name,
                        CreatedAt = company.CreatedAt
                    }
                );
        });
    }

    public async Task<Either<CompanyFetchingError, IntIdOutputModel>> DeleteCompanyAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var company = await companyRepository.DeleteCompanyAsync(id);
            return company is null
                ? EitherExtensions.Failure<CompanyFetchingError, IntIdOutputModel>(
                    new CompanyFetchingError.CompanyByIdNotFound(id)
                )
                : EitherExtensions.Success<CompanyFetchingError, IntIdOutputModel>(
                    new IntIdOutputModel(company.Id)
                );
        });
    }
}
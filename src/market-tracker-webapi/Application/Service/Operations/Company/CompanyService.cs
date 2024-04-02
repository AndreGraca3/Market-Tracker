using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Http.Models;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Repository.Operations.Company;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Services.Errors.Company;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Company;

public class CompanyService(
    ICompanyRepository companyRepository,
    ITransactionManager transactionManager
) : ICompanyService
{
    public async Task<CollectionOutputModel> GetCompaniesAsync()
    {
        var companies = await companyRepository.GetCompaniesAsync();
        return new CollectionOutputModel(companies);
    }

    public async Task<Either<CompanyFetchingError, Domain.Company>> GetCompanyByIdAsync(int id)
    {
        var company = await companyRepository.GetCompanyByIdAsync(id);
        return company is null
            ? EitherExtensions.Failure<CompanyFetchingError, Domain.Company>(
                new CompanyFetchingError.CompanyByIdNotFound(id)
            )
            : EitherExtensions.Success<CompanyFetchingError, Domain.Company>(company);
    }

    public async Task<Either<CompanyFetchingError, Domain.Company>> GetCompanyByNameAsync(
        string companyName
    )
    {
        var company = await companyRepository.GetCompanyByNameAsync(companyName);
        return company is null
            ? EitherExtensions.Failure<CompanyFetchingError, Domain.Company>(
                new CompanyFetchingError.CompanyByNameNotFound(companyName)
            )
            : EitherExtensions.Success<CompanyFetchingError, Domain.Company>(company);
    }

    public async Task<Either<ICompanyError, IdOutputModel>> AddCompanyAsync(string companyName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await companyRepository.GetCompanyByNameAsync(companyName) is not null)
            {
                return EitherExtensions.Failure<ICompanyError, IdOutputModel>(
                    new CompanyCreationError.CompanyNameAlreadyExists(companyName)
                );
            }

            var companyId = await companyRepository.AddCompanyAsync(companyName);
            return EitherExtensions.Success<ICompanyError, IdOutputModel>(
                new IdOutputModel(companyId)
            );
        });
    }

    public async Task<Either<ICompanyError, Domain.Company>> UpdateCompanyAsync(
        int id,
        string companyName
    )
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await companyRepository.GetCompanyByNameAsync(companyName) is not null)
            {
                return EitherExtensions.Failure<ICompanyError, Domain.Company>(
                    new CompanyCreationError.CompanyNameAlreadyExists(companyName)
                );
            }

            var company = await companyRepository.UpdateCompanyAsync(id, companyName);
            return company is null
                ? EitherExtensions.Failure<ICompanyError, Domain.Company>(
                    new CompanyFetchingError.CompanyByIdNotFound(id)
                )
                : EitherExtensions.Success<ICompanyError, Domain.Company>(
                    new Domain.Company
                    {
                        Id = company.Id,
                        Name = company.Name,
                        CreatedAt = company.CreatedAt
                    }
                );
        });
    }

    public async Task<Either<CompanyFetchingError, IdOutputModel>> DeleteCompanyAsync(int id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var company = await companyRepository.DeleteCompanyAsync(id);
            return company is null
                ? EitherExtensions.Failure<CompanyFetchingError, IdOutputModel>(
                    new CompanyFetchingError.CompanyByIdNotFound(id)
                )
                : EitherExtensions.Success<CompanyFetchingError, IdOutputModel>(
                    new IdOutputModel(company.Id)
                );
        });
    }
}

using market_tracker_webapi.Application.Domain;
using market_tracker_webapi.Application.Models;
using market_tracker_webapi.Application.Repository.Operations.Company;
using market_tracker_webapi.Application.Service.Errors.Company;
using market_tracker_webapi.Application.Services.Errors.Company;
using market_tracker_webapi.Application.Services.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Company;

public class CompanyService(ICompanyRepository companyRepository, ITransactionManager transactionManager) : ICompanyService
{
    public async Task<IEnumerable<CompanyDomain>> GetCompaniesAsync()
    {
        return await companyRepository.GetCompaniesAsync();
    }

    public async Task<Either<CompanyFetchingError, CompanyDomain>> GetCompanyByIdAsync(int id)
    {
        var company = await companyRepository.GetCompanyByIdAsync(id);
        return company is null
            ? EitherExtensions.Failure<CompanyFetchingError, CompanyDomain>(
                new CompanyFetchingError.CompanyByIdNotFound(id)
            )
            : EitherExtensions.Success<CompanyFetchingError, CompanyDomain>(company);
    }

    public async Task<Either<CompanyFetchingError, CompanyDomain>> GetCompanyByNameAsync(string companyName)
    {
        var company = await companyRepository.GetCompanyByNameAsync(companyName);
        return company is null
            ? EitherExtensions.Failure<CompanyFetchingError, CompanyDomain>(
                new CompanyFetchingError.CompanyByNameNotFound(companyName)
            )
            : EitherExtensions.Success<CompanyFetchingError, CompanyDomain>(company);
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
                new IdOutputModel
                {
                    Id = companyId
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
                    new IdOutputModel
                    {
                        Id = company.Id
                    }
                );
        });
    }

    public async Task<Either<ICompanyError, CompanyDomain>> UpdateCompanyAsync(int id, string companyName)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await companyRepository.GetCompanyByNameAsync(companyName) is not null)
            {
                return EitherExtensions.Failure<ICompanyError, CompanyDomain>(
                    new CompanyCreationError.CompanyNameAlreadyExists(companyName)
                );
            }
            
            var company = await companyRepository.UpdateCompanyAsync(id, companyName);
            return company is null
                ? EitherExtensions.Failure<ICompanyError, CompanyDomain>(
                    new CompanyFetchingError.CompanyByIdNotFound(id)
                )
                : EitherExtensions.Success<ICompanyError, CompanyDomain>(
                    new CompanyDomain
                    {
                        Id = company.Id,
                        Name = company.Name,
                        CreatedAt = company.CreatedAt
                    }
                );
        });
    }
}
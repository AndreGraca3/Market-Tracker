using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Account.Users.Operator;
using market_tracker_webapi.Application.Repository.Account.Users.User;
using market_tracker_webapi.Application.Repository.Market.City;
using market_tracker_webapi.Application.Repository.Market.Company;
using market_tracker_webapi.Application.Repository.Market.Store;
using market_tracker_webapi.Application.Repository.Market.Store.PreRegister;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.PreRegister;
using market_tracker_webapi.Application.Service.Errors.User;
using market_tracker_webapi.Application.Service.Transaction;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.Operator;

using Operator = Domain.Schemas.Account.Users.Operator;

public class OperatorService(
    ICityRepository cityRepository,
    IStoreRepository storeRepository,
    ICompanyRepository companyRepository,
    IPreRegistrationRepository preRegistrationRepository,
    IUserRepository userRepository,
    IOperatorRepository operatorRepository,
    ITransactionManager transactionManager
) : IOperatorService
{
    public async Task<PaginatedResult<OperatorItem>> GetOperatorsAsync(int skip,
        int take)
    {
        return await operatorRepository.GetOperatorsAsync(skip, take);
    }

    public async Task<Operator> GetOperatorByIdAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await operatorRepository.GetOperatorByIdAsync(id) ??
            throw new MarketTrackerServiceException(new UserFetchingError.UserByIdNotFound(id))
        );
    }

    public async Task<UserId> CreateOperatorAsync(Guid id, string password)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var preRegistrationOperator = await preRegistrationRepository.GetPreRegisterByIdAsync(id);
            if (preRegistrationOperator is null)
            {
                throw new MarketTrackerServiceException(
                    new PreRegistrationFetchingError.PreRegistrationByIdNotFound(id)
                );
            }

            if (!preRegistrationOperator.IsValidated)
            {
                throw new MarketTrackerServiceException(
                    new PreRegistrationFetchingError.PreRegistrationNotValidated(id)
                );
            }

            await preRegistrationRepository.DeletePreRegisterAsync(id);

            var userId = await userRepository.CreateUserAsync(preRegistrationOperator.OperatorName,
                preRegistrationOperator.Email,
                Role.Operator.ToString());

            await operatorRepository.CreateOperatorAsync(userId.Value, preRegistrationOperator.PhoneNumber);
            // create company
            var companyId = (await companyRepository.AddCompanyAsync(preRegistrationOperator.CompanyName)).Value;
            // create city
            var cityId = preRegistrationOperator.CityName is not null
                ? (await cityRepository.AddCityAsync(preRegistrationOperator.CityName)).Value
                : (int?)null;
            // create Store
            await storeRepository.AddStoreAsync(preRegistrationOperator.StoreName,
                preRegistrationOperator.StoreAddress, cityId, companyId);

            return userId;
        });
    }

    public async Task<Operator> UpdateOperatorAsync(Guid id, int phoneNumber)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var user = await userRepository.GetUserByIdAsync(id);

            if (await operatorRepository.GetOperatorByIdAsync(id) is not null)
            {
                await operatorRepository.UpdateOperatorAsync(id, phoneNumber); // TODO: discuss this, why update?
            }
            else
            {
                await operatorRepository.CreateOperatorAsync(id, phoneNumber);
            }

            return new Operator(user!, phoneNumber); // discuss this, double bang in service auth cases?
        });
    }

    public async Task<UserId> DeleteOperatorAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
            (await userRepository.DeleteUserAsync(id))?.Id ??
            throw new MarketTrackerServiceException(new UserFetchingError.UserByIdNotFound(id))
        );
    }
}
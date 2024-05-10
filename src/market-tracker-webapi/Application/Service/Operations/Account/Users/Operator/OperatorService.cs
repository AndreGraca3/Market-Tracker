using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Models.Account.Users;
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
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.Operator;

using Operator = Domain.Models.Account.Users.Operator;

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
    public async Task<Either<IServiceError, PaginatedResult<OperatorItem>>> GetOperatorsAsync(int skip,
        int take)
    {
        return EitherExtensions.Success<IServiceError, PaginatedResult<OperatorItem>>(
            await operatorRepository.GetOperatorsAsync(skip, take)
        );
    }

    public async Task<Either<UserFetchingError, Operator>> GetOperatorByIdAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var operatorInfo = await operatorRepository.GetOperatorByIdAsync(id);
            if (operatorInfo is null)
            {
                return EitherExtensions.Failure<UserFetchingError, Operator>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<UserFetchingError, Operator>(
                operatorInfo
            );
        });
    }

    public async Task<Either<PreRegistrationFetchingError, UserId>> CreateOperatorAsync(Guid id,
        string password)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var preRegistrationOperator = await preRegistrationRepository.GetPreRegisterByIdAsync(id);
            if (preRegistrationOperator is null)
            {
                return EitherExtensions.Failure<PreRegistrationFetchingError, UserId>(
                    new PreRegistrationFetchingError.PreRegistrationByIdNotFound(id)
                );
            }

            if (!preRegistrationOperator.IsValidated)
            {
                return EitherExtensions.Failure<PreRegistrationFetchingError, UserId>(
                    new PreRegistrationFetchingError.PreRegistrationNotValidated(id)
                );
            }

            await preRegistrationRepository.DeletePreRegisterAsync(id);

            var userId = (await userRepository.CreateUserAsync(preRegistrationOperator.OperatorName,
                preRegistrationOperator.Email,
                Role.Operator.ToString())).Value;

            await operatorRepository.CreateOperatorAsync(userId, preRegistrationOperator.PhoneNumber);
            // create company
            var companyId = (await companyRepository.AddCompanyAsync(preRegistrationOperator.CompanyName)).Value;
            // create city
            var cityId = preRegistrationOperator.CityName is not null
                ? (await cityRepository.AddCityAsync(preRegistrationOperator.CityName)).Value
                : (int?)null;
            // create Store
            await storeRepository.AddStoreAsync(preRegistrationOperator.StoreName,
                preRegistrationOperator.StoreAddress, cityId, companyId);

            return EitherExtensions.Success<PreRegistrationFetchingError, UserId>(
                new UserId(userId)
            );
        });
    }

    public async Task<Either<UserFetchingError, Operator>> UpdateOperatorAsync(Guid id, int phoneNumber)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var user = await userRepository.GetUserByIdAsync(id);

            if (user is null)
            {
                return EitherExtensions.Failure<UserFetchingError, Operator>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            if (await operatorRepository.GetOperatorByIdAsync(id) is not null)
            {
                await operatorRepository.UpdateOperatorAsync(
                    id,
                    phoneNumber
                );
            }
            else
            {
                await operatorRepository.CreateOperatorAsync(id, phoneNumber);
            }

            return EitherExtensions.Success<UserFetchingError, Operator>(
                new Operator(user, phoneNumber)
            );
        });
    }

    public async Task<Either<UserFetchingError, UserId>> DeleteOperatorAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var user = await userRepository.DeleteUserAsync(id);
            if (user is null)
            {
                return EitherExtensions.Failure<UserFetchingError, UserId>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<UserFetchingError, UserId>(
                user.Id
            );
        });
    }
}
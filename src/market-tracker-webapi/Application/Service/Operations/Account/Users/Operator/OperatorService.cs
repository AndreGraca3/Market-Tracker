using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.Operator;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Application.Repository.Market.City;
using market_tracker_webapi.Application.Repository.Market.Store;
using market_tracker_webapi.Application.Repository.Operations.Account.Users.Operator;
using market_tracker_webapi.Application.Repository.Operations.Account.Users.User;
using market_tracker_webapi.Application.Repository.Operations.Market.Company;
using market_tracker_webapi.Application.Repository.Operations.Market.Store;
using market_tracker_webapi.Application.Repository.Operations.Market.Store.PreRegister;
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
    public async Task<Either<IServiceError, PaginatedResult<OperatorOutputModel>>> GetOperatorsAsync(bool? isApproved, int skip,
        int take)
    {
        return EitherExtensions.Success<IServiceError, PaginatedResult<OperatorOutputModel>>(
            await preRegistrationRepository.GetPreRegistersAsync(isApproved, skip, take)
        );
    }

    public async Task<Either<UserFetchingError, OperatorInfo>> GetOperatorByIdAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var operatorInfo = await operatorRepository.GetOperatorByIdAsync(id);
            if (operatorInfo is null)
            {
                return EitherExtensions.Failure<UserFetchingError, OperatorInfo>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<UserFetchingError, OperatorInfo>(
                operatorInfo
            );
        });
    }

    public async Task<Either<PreRegistrationFetchingError, GuidOutputModel>> CreateOperatorAsync(Guid code, string password)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var preRegistrationOperator = await preRegistrationRepository.GetPreRegisterByIdAsync(code);
            if (preRegistrationOperator is null)
            {
                return EitherExtensions.Failure<PreRegistrationFetchingError, GuidOutputModel>(
                    new PreRegistrationFetchingError.PreRegistrationByIdNotFound(code)
                );
            }

            if (!preRegistrationOperator.IsValidated)
            {
                return EitherExtensions.Failure<PreRegistrationFetchingError, GuidOutputModel>(
                    new PreRegistrationFetchingError.PreRegistrationNotValidated(code)
                );
            }

            await preRegistrationRepository.DeletePreRegisterAsync(code);

            var userId = await userRepository.CreateUserAsync(preRegistrationOperator.OperatorName,
                preRegistrationOperator.Email,
                Role.Operator.ToString());

            await operatorRepository.CreateOperatorAsync(userId, preRegistrationOperator.PhoneNumber);
            // create company
            var companyId = await companyRepository.AddCompanyAsync(preRegistrationOperator.CompanyName);
            // create city
            var cityId = preRegistrationOperator.CityName is not null
                ? await cityRepository.AddCityAsync(preRegistrationOperator.CityName)
                : (int?)null;
            // create Store
            await storeRepository.AddStoreAsync(preRegistrationOperator.StoreName,
                preRegistrationOperator.StoreAddress, cityId, companyId);

            return EitherExtensions.Success<PreRegistrationFetchingError, GuidOutputModel>(
                new GuidOutputModel(userId)
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
                new Operator(id, phoneNumber)
            );
        });
    }

    public async Task<Either<UserFetchingError, GuidOutputModel>> DeleteOperatorAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var user = await userRepository.DeleteUserAsync(id);
            if (user is null)
            {
                return EitherExtensions.Failure<UserFetchingError, GuidOutputModel>(
                    new UserFetchingError.UserByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<UserFetchingError, GuidOutputModel>(
                new GuidOutputModel(
                    user.Id
                )
            );
        });
    }
}
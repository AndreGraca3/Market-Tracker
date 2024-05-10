using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Models.Account.Auth;
using market_tracker_webapi.Application.Domain.Models.Account.Users;
using market_tracker_webapi.Application.Repository.Account.Users.Operator;
using market_tracker_webapi.Application.Repository.Market.Store.PreRegister;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.PreRegister;
using market_tracker_webapi.Application.Service.Transaction;
using market_tracker_webapi.Application.Utils;

namespace market_tracker_webapi.Application.Service.Operations.Account.Auth.PreRegister;

public class PreRegistrationService(
    IPreRegistrationRepository preRegistrationRepository,
    IOperatorRepository operatorRepository,
    ITransactionManager transactionManager
) : IPreRegistrationService
{
    public async Task<Either<IServiceError, PaginatedResult<OperatorItem>>> GetPreRegistrationsAsync(bool? isValid,
        int skip, int limit)
    {
        return await transactionManager.ExecuteAsync(async () =>
            EitherExtensions.Success<IServiceError, PaginatedResult<OperatorItem>>(
                await preRegistrationRepository.GetPreRegistersAsync(isValid, skip, limit)
            ));
    }

    public async Task<Either<PreRegistrationFetchingError, PreRegistration>> GetPreRegistrationByIdAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var preRegistrationOperator = await preRegistrationRepository.GetPreRegisterByIdAsync(id);
            if (preRegistrationOperator is null)
            {
                return EitherExtensions.Failure<PreRegistrationFetchingError, PreRegistration>(
                    new PreRegistrationFetchingError.PreRegistrationByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<PreRegistrationFetchingError, PreRegistration>(
                preRegistrationOperator
            );
        });
    }

    public async Task<Either<PreRegistrationCreationError, PreRegistrationId>> AddPreRegistrationAsync(
        string operatorName,
        string email,
        int phoneNumber,
        string storeName,
        string storeAddress,
        string companyName,
        string? cityName,
        string document)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            if (await preRegistrationRepository.GetPreRegisterByEmail(email) is not null)
            {
                return EitherExtensions.Failure<PreRegistrationCreationError, PreRegistrationId>(
                    new PreRegistrationCreationError.EmailAlreadyInUse(email)
                );
            }

            if (await operatorRepository.GetOperatorByEmailAsync(email) is not null)
            {
                return EitherExtensions.Failure<PreRegistrationCreationError, PreRegistrationId>(
                    new PreRegistrationCreationError.EmailAlreadyInUse(email)
                );
            }

            var code = await preRegistrationRepository.CreatePreRegisterAsync(
                operatorName,
                email,
                phoneNumber,
                storeName,
                storeAddress,
                companyName,
                cityName,
                document
            );

            return EitherExtensions.Success<PreRegistrationCreationError, PreRegistrationId>(code);
        });
    }

    public async Task<Either<PreRegistrationFetchingError, PreRegistrationId>> UpdatePreRegistrationByIdAsync(Guid id,
        bool isApproved)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var preRegistrationOperator = await preRegistrationRepository.UpdatePreRegistrationById(id, isApproved);

            if (preRegistrationOperator is null)
            {
                return EitherExtensions.Failure<PreRegistrationFetchingError, PreRegistrationId>(
                    new PreRegistrationFetchingError.PreRegistrationByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<PreRegistrationFetchingError, PreRegistrationId>(
                new PreRegistrationId(id)
            );
        });
    }
}
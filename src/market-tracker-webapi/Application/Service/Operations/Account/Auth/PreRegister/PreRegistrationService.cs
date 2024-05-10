using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Http.Models.Identifiers;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Operator;
using market_tracker_webapi.Application.Http.Models.Schemas.Market.Retail.Store;
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
    public async Task<Either<IServiceError, PaginatedResult<OperatorOutputModel>>> GetPreRegistrationsAsync(bool? isValid,
        int skip, int limit)
    {
        return await transactionManager.ExecuteAsync(async () =>
            EitherExtensions.Success<IServiceError, PaginatedResult<OperatorOutputModel>>(
                await preRegistrationRepository.GetPreRegistersAsync(isValid, skip, limit)
            ));
    }

    public async Task<Either<PreRegistrationFetchingError, PreRegisterInfo>> GetPreRegistrationByIdAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var preRegistrationOperator = await preRegistrationRepository.GetPreRegisterByIdAsync(id);
            if (preRegistrationOperator is null)
            {
                return EitherExtensions.Failure<PreRegistrationFetchingError, PreRegisterInfo>(
                    new PreRegistrationFetchingError.PreRegistrationByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<PreRegistrationFetchingError, PreRegisterInfo>(
                PreRegisterInfo.ToPreRegisterInfo(preRegistrationOperator)
            );
        });
    }

    public async Task<Either<PreRegistrationCreationError, GuidOutputModel>> AddPreRegistrationAsync(
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
                return EitherExtensions.Failure<PreRegistrationCreationError, GuidOutputModel>(
                    new PreRegistrationCreationError.EmailAlreadyInUse(email)
                );
            }

            if (await operatorRepository.GetOperatorByEmailAsync(email) is not null)
            {
                return EitherExtensions.Failure<PreRegistrationCreationError, GuidOutputModel>(
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

            return EitherExtensions.Success<PreRegistrationCreationError, GuidOutputModel>(
                new GuidOutputModel(code)
            );
        });
    }

    public async Task<Either<PreRegistrationFetchingError, GuidOutputModel>> UpdatePreRegistrationByIdAsync(Guid id,
        bool isApproved)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var preRegistrationOperator = await preRegistrationRepository.UpdatePreRegistrationById(id, isApproved);

            if (preRegistrationOperator is null)
            {
                return EitherExtensions.Failure<PreRegistrationFetchingError, GuidOutputModel>(
                    new PreRegistrationFetchingError.PreRegistrationByIdNotFound(id)
                );
            }

            return EitherExtensions.Success<PreRegistrationFetchingError, GuidOutputModel>(
                new GuidOutputModel(id)
            );
        });
    }
}
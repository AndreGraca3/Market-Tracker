using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Auth;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Repository.Account.Users.Operator;
using market_tracker_webapi.Application.Repository.Market.Store.PreRegister;
using market_tracker_webapi.Application.Service.Errors;
using market_tracker_webapi.Application.Service.Errors.PreRegister;
using market_tracker_webapi.Application.Service.Transaction;

namespace market_tracker_webapi.Application.Service.Operations.Account.Auth.PreRegister;

public class PreRegistrationService(
    IPreRegistrationRepository preRegistrationRepository,
    IOperatorRepository operatorRepository,
    ITransactionManager transactionManager
) : IPreRegistrationService
{
    public async Task<PaginatedResult<OperatorItem>> GetPreRegistrationsAsync(bool? isValid,
        int skip, int limit)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await preRegistrationRepository.GetPreRegistersAsync(isValid, skip, limit)
        );
    }

    public async Task<PreRegistration> GetPreRegistrationByIdAsync(Guid id)
    {
        return await transactionManager.ExecuteAsync(async () =>
            await preRegistrationRepository.GetPreRegisterByIdAsync(id) ?? throw new MarketTrackerServiceException(
                new PreRegistrationFetchingError.PreRegistrationByIdNotFound(id)
            ));
    }

    public async Task<PreRegistrationCode> AddPreRegistrationAsync(
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
                throw new MarketTrackerServiceException(
                    new PreRegistrationCreationError.EmailAlreadyInUse(email)
                );
            }

            if (await operatorRepository.GetOperatorByEmailAsync(email) is not null)
            {
                throw new MarketTrackerServiceException(
                    new PreRegistrationCreationError.EmailAlreadyInUse(email)
                );
            }

            return await preRegistrationRepository.CreatePreRegisterAsync(
                operatorName,
                email,
                phoneNumber,
                storeName,
                storeAddress,
                companyName,
                cityName,
                document
            );
        });
    }

    public async Task<PreRegistrationCode> UpdatePreRegistrationByIdAsync(Guid id,
        bool isApproved)
    {
        return await transactionManager.ExecuteAsync(async () =>
        {
            var preRegistration = await preRegistrationRepository.UpdatePreRegistrationById(id, isApproved);

            if (preRegistration is null)
            {
                throw new MarketTrackerServiceException(
                    new PreRegistrationFetchingError.PreRegistrationByIdNotFound(id)
                );
            }

            return preRegistration.Code;
        });
    }
}
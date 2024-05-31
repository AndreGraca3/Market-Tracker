using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Application.Service.Operations.Account.Users.Operator;

using Operator = Domain.Schemas.Account.Users.Operator;

public interface IOperatorService
{
    Task<PaginatedResult<OperatorItem>> GetOperatorsAsync(int skip, int take);

    Task<Operator> GetOperatorByIdAsync(Guid id);

    Task<UserId> CreateOperatorAsync(Guid code, string password);

    Task<Operator> UpdateOperatorAsync(Guid id, int phoneNumber);

    Task<UserId> DeleteOperatorAsync(Guid id);
}
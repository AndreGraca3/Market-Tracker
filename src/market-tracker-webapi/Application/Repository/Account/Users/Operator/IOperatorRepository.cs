using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;

namespace market_tracker_webapi.Application.Repository.Account.Users.Operator;

using Operator = Domain.Schemas.Account.Users.Operator;

public interface IOperatorRepository
{
    Task<PaginatedResult<OperatorItem>> GetOperatorsAsync(int skip, int take);

    Task<Operator?> GetOperatorByIdAsync(Guid id);

    Task<Operator?> GetOperatorByEmailAsync(string email);

    Task<UserId> CreateOperatorAsync(Guid userId, int phoneNumber);

    Task<Operator?> UpdateOperatorAsync(Guid id, int phoneNumber);
}
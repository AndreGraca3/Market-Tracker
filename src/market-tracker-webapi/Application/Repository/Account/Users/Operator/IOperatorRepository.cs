using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Http.Models.Schemas.Account.Users.Operator;

namespace market_tracker_webapi.Application.Repository.Account.Users.Operator;

using Operator = Domain.Models.Account.Users.Operator;

public interface IOperatorRepository
{
    Task<PaginatedResult<OperatorOutputModel>> GetOperatorsAsync(int skip, int take);

    Task<Operator?> GetOperatorByIdAsync(Guid id);

    Task<Operator?> GetOperatorByEmailAsync(string email);

    Task<Guid> CreateOperatorAsync(Guid userId, int phoneNumber);

    Task<Operator?> UpdateOperatorAsync(Guid id, int phoneNumber);
}
using market_tracker_webapi.Application.Repository.Dto;
using market_tracker_webapi.Application.Repository.Dto.Operator;

namespace market_tracker_webapi.Application.Repository.Operations.Operator;

using Operator = Domain.Operator;

public interface IOperatorRepository
{
    Task<PaginatedResult<OperatorItem>> GetOperatorsAsync(int skip, int take);

    Task<OperatorInfo?> GetOperatorByIdAsync(Guid id);

    Task<Operator?> GetOperatorByEmailAsync(string email);

    Task<Guid> CreateOperatorAsync(Guid userId, int phoneNumber);

    Task<Operator?> UpdateOperatorAsync(Guid id, int phoneNumber);
}
using market_tracker_webapi.Application.Repository.Dto.Operator;

namespace market_tracker_webapi.Application.Repository.Operations.Operator;

using Operator = Domain.Operator;

public interface IOperatorRepository
{
    Task<OperatorInfo?> GetOperatorByIdAsync(Guid id);
    
    Task<Guid> CreateOperatorAsync(Guid userId, int phoneNumber);

    Task<Operator?> UpdateOperatorAsync(Guid id, int phoneNumber);
    
    Task<Operator?> DeleteOperatorAsync(Guid id);
}
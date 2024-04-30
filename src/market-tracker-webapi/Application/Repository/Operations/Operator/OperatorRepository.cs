using market_tracker_webapi.Application.Repository.Dto.Operator;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Operator;

using Operator = Domain.Operator;

public class OperatorRepository(
    MarketTrackerDataContext dataContext
) : IOperatorRepository
{
    public async Task<OperatorInfo?> GetOperatorByIdAsync(Guid id)
    {
        var query = from user in dataContext.User
            join oper in dataContext.Operator on user.Id equals oper.UserId
            where user.Role == "client" && user.Id == id
            select new OperatorInfo(user.Id, user.Username, user.Name, user.Email, user.CreatedAt, oper.PhoneNumber);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<Guid> CreateOperatorAsync(Guid userId, int phoneNumber)
    {
        var newOperator = new OperatorEntity
        {
            UserId = userId,
            PhoneNumber = phoneNumber
        };
        await dataContext.Operator.AddAsync(newOperator);
        await dataContext.SaveChangesAsync();
        return newOperator.UserId;
    }

    public async Task<Operator?> UpdateOperatorAsync(Guid id, int phoneNumber)
    {
        var operatorEntity = await dataContext.Operator.FindAsync(id);
        if (operatorEntity is null)
        {
            return null;
        }

        operatorEntity.PhoneNumber = phoneNumber;

        await dataContext.SaveChangesAsync();
        return operatorEntity.ToOperator();
    }

    public async Task<Operator?> DeleteOperatorAsync(Guid id)
    {
        var deletedOperatorEntity = await dataContext.Operator.FindAsync(id);
        if (deletedOperatorEntity is null)
        {
            return null;
        }

        dataContext.Remove(deletedOperatorEntity);
        await dataContext.SaveChangesAsync();
        return deletedOperatorEntity.ToOperator();
    }
}
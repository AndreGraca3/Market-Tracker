using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Http.Models.Operator;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Users;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Operations.Account.Users.Operator;

using Operator = Domain.Models.Account.Users.Operator;

public class OperatorRepository(
    MarketTrackerDataContext dataContext
) : IOperatorRepository
{
    public async Task<PaginatedResult<OperatorOutputModel>> GetOperatorsAsync(int skip, int take)
    {
        var query = from user in dataContext.User
            join oper in dataContext.Operator on user.Id equals oper.UserId
            join store in dataContext.Store on oper.UserId equals store.OperatorId
            where user.Role == Role.Operator.ToString()
            select new OperatorOutputModel(user.Id, user.Name, oper.PhoneNumber, store.Name, user.CreatedAt);

        var operators = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return new PaginatedResult<OperatorOutputModel>(operators, query.Count(), skip, take);
    }

    public async Task<OperatorInfo?> GetOperatorByIdAsync(Guid id)
    {
        var query = from user in dataContext.User
            join oper in dataContext.Operator on user.Id equals oper.UserId
            where user.Role == Role.Operator.ToString() && user.Id == id
            select new OperatorInfo(user.Id, user.Name, user.Email, oper.PhoneNumber, user.CreatedAt);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<Operator?> GetOperatorByEmailAsync(string email)
    {
        var query = from user in dataContext.User
            join oper in dataContext.Operator on user.Id equals oper.UserId
            join store in dataContext.Store on oper.UserId equals store.OperatorId
            where user.Role == Role.Operator.ToString()
            select new Operator(user.Id, oper.PhoneNumber);

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
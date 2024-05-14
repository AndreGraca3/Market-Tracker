using market_tracker_webapi.Application.Domain.Filters;
using market_tracker_webapi.Application.Domain.Schemas.Account.Users;
using market_tracker_webapi.Application.Http.Pipeline.Authorization;
using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Users;
using Microsoft.EntityFrameworkCore;

namespace market_tracker_webapi.Application.Repository.Account.Users.Operator;

using Operator = Domain.Schemas.Account.Users.Operator;

public class OperatorRepository(
    MarketTrackerDataContext dataContext
) : IOperatorRepository
{
    public async Task<PaginatedResult<OperatorItem>> GetOperatorsAsync(int skip, int take)
    {
        var query = from user in dataContext.User
            join @operator in dataContext.Operator on user.Id equals @operator.UserId
            join store in dataContext.Store on @operator.UserId equals store.OperatorId
            where user.Role == Role.Operator.ToString()
            select new OperatorItem(user.Id, user.Name, @operator.PhoneNumber, store.Name);

        var operators = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return new PaginatedResult<OperatorItem>(operators, query.Count(), skip, take);
    }

    public async Task<Operator?> GetOperatorByIdAsync(Guid id)
    {
        var query = from user in dataContext.User
            join @operator in dataContext.Operator on user.Id equals @operator.UserId
            where user.Role == Role.Operator.ToString() && user.Id == id
            select new Operator(user.Id, user.Name, user.Email, @operator.PhoneNumber, user.CreatedAt);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<Operator?> GetOperatorByEmailAsync(string email)
    {
        var query = from user in dataContext.User
            join @operator in dataContext.Operator on user.Id equals @operator.UserId
            join store in dataContext.Store on @operator.UserId equals store.OperatorId
            where user.Role == Role.Operator.ToString()
            select new Operator(user.ToUser(), @operator.PhoneNumber);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<UserId> CreateOperatorAsync(Guid userId, int phoneNumber)
    {
        var newOperator = new OperatorEntity
        {
            UserId = userId,
            PhoneNumber = phoneNumber
        };
        await dataContext.Operator.AddAsync(newOperator);
        await dataContext.SaveChangesAsync();
        return new UserId(newOperator.UserId);
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
        return await GetOperatorByIdAsync(id);
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
        return await GetOperatorByIdAsync(id);
    }
}
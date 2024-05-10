using market_tracker_webapi.Infrastructure;
using market_tracker_webapi.Infrastructure.PostgreSQLTables.Account.Auth;

namespace market_tracker_webapi.Application.Repository.Account.Credential;

using Account = Domain.Models.Account.Auth.Account;

public class AccountRepository(
    MarketTrackerDataContext dataContext
) : IAccountRepository
{
    public async Task<Account?> GetPasswordByUserIdAsync(Guid userId)
    {
        return (await dataContext.Account.FindAsync(userId))?.ToAccount();
    }

    public async Task<Account> CreatePasswordAsync(Guid userId, string password)
    {
        var accountEntity = new AccountEntity
        {
            UserId = userId,
            Password = password
        };

        await dataContext.Account.AddAsync(accountEntity);
        await dataContext.SaveChangesAsync();
        return accountEntity.ToAccount();
    }

    public async Task<Account?> UpdatePasswordAsync(Guid userId, string newPassword)
    {
        var accountEntity = await dataContext.Account.FindAsync(userId);
        if (accountEntity is null)
        {
            return null;
        }

        accountEntity.Password = newPassword;

        await dataContext.SaveChangesAsync();
        return accountEntity.ToAccount();
    }

    public async Task<Account?> DeletePasswordAsync(Guid userId)
    {
        var deletedAccountEntity = await dataContext.Account.FindAsync(userId);
        if (deletedAccountEntity is null)
        {
            return null;
        }

        dataContext.Remove(deletedAccountEntity);
        await dataContext.SaveChangesAsync();
        return deletedAccountEntity.ToAccount();
    }
}
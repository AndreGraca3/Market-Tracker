namespace market_tracker_webapi.Application.Repository.Operations.Account.Credential;

using Account = Domain.Models.Account.Auth.Account;

public interface IAccountRepository
{
    Task<Account?> GetPasswordByUserIdAsync(Guid userId);

    Task<Account> CreatePasswordAsync(Guid userId, string password);

    Task<Account?> UpdatePasswordAsync(Guid userId, string newPassword);

    Task<Account?> DeletePasswordAsync(Guid userId);
}
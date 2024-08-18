using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Contract.Services.Interface
{
    public interface IAccountService
    {
        Task<Account> GetAccountByIdAsync(string id);
        Task<IEnumerable<Account>> GetAccountsByRoleAsync(string role);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task UpdateAccountByIdAsync(string id, Account updatedAccount);
        Task UpdateAccountRoleAsync(string accountId, string newRole);
    }
}
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Contract.Services.Interface
{
    public interface IAccountService
    {
        Task<Account> GetAccountByIdAsync(string id);
        Task<IEnumerable<Account>> GetAccountsByRoleAsync(string role);
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account> UpdateAccountAsync(string id, Account account);
        Task<bool> DeleteAccountAsync(string id);
        Task UpdateAccountRoleAsync(string accountId, string newRole);
    }
}
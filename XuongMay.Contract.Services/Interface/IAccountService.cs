using XuongMay.Contract.Repositories.Entity;

public interface IAccountService
{
    Task<Account> GetAccountByIdAsync(string id);
    Task<IEnumerable<Account>> GetAccountsByRoleAsync(string role);
    Task<IEnumerable<Account>> GetAllAccountsAsync();
    Task UpdateAccountByIdAsync(string id, Account updatedAccount);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Contract.Services.Interface
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account> GetAccountByIdAsync(string id);
        Task<Account> CreateAccountAsync(Account account);
        Task<Account> UpdateAccountAsync(string id, Account account);
        Task<bool> DeleteAccountAsync(string id);
    }
}

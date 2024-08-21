using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Contract.Repositories.Interface
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account> GetByIdAsync(Guid id);
        Task<Account> GetByPredicateAsync(Func<Account, bool> predicate);
        Task InsertAsync(Account account);
        Task<bool> UpdateAsync(Guid id, Account account);
        Task<bool> DeleteAsync(Guid id);
    }
}

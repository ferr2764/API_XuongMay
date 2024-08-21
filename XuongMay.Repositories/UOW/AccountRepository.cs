using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;

namespace XuongMay.Repositories.UOW
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<Account> _accounts;

        public AccountRepository(DbContext context)
        {
            _context = context;
            _accounts = context.Set<Account>();
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _accounts.ToListAsync();
        }

        public async Task<Account> GetByIdAsync(Guid id)
        {
            return await _accounts.FindAsync(id);
        }

        public async Task<Account> GetByPredicateAsync(Func<Account, bool> predicate)
        {
            return await Task.FromResult(_accounts.SingleOrDefault(predicate));
        }

        public async Task InsertAsync(Account account)
        {
            await _accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Guid id, Account account)
        {
            var existingAccount = await _accounts.FindAsync(id);
            if (existingAccount == null) return false;

            _context.Entry(existingAccount).CurrentValues.SetValues(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var account = await _accounts.FindAsync(id);
            if (account == null) return false;

            _accounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

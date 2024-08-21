using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.AccountModelView;
using XuongMay.ModelViews.AuthModelViews;

namespace XuongMay.Services.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        private ExposeAccountModelView AccountMapping(Account account)
        {
            return new ExposeAccountModelView
            {
                Id = account.AccountId,
                Name = account.Name,
                Role = account.Role,
                Username = account.Username,
                Salary = account.Salary,
                Status = account.Status
            };
        }

        public async Task<ExposeAccountModelView> GetAccountByIdAsync(Guid id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            return account == null ? null : AccountMapping(account);
        }

        public async Task<IEnumerable<ExposeAccountModelView>> GetAccountsByRoleAsync(string role)
        {
            var accounts = await _accountRepository.GetAllAsync();
            var filteredAccounts = accounts.Where(a => a.Role == role);
            return filteredAccounts.Select(AccountMapping);
        }

        public async Task<IEnumerable<ExposeAccountModelView>> GetAllAccountsAsync(int pageNumber = 1, int pageSize = 5)
        {
            var accounts = await _accountRepository.GetAllAsync();
            var pagedAccounts = accounts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return pagedAccounts.Select(AccountMapping);
        }

        public async Task<ExposeAccountModelView> UpdateAccountAsync(Guid id, UpdateAccountModelView accountModelView)
        {
            var existingAccount = await _accountRepository.GetByIdAsync(id);
            if (existingAccount == null) return null;

            existingAccount.Name = accountModelView.Name;
            existingAccount.Username = accountModelView.Username;
            existingAccount.Password = BCrypt.Net.BCrypt.HashPassword(accountModelView.Password);
            existingAccount.Salary = accountModelView.Salary;

            await _accountRepository.UpdateAsync(id, existingAccount);
            return AccountMapping(existingAccount);
        }

        public async Task<bool> DeleteAccountAsync(Guid id)
        {
            var existingAccount = await _accountRepository.GetByIdAsync(id);
            if (existingAccount == null) return false;

            existingAccount.Status = "Unavailable";
            await _accountRepository.UpdateAsync(id, existingAccount);
            return true;
        }

        public async Task UpdateAccountRoleAsync(Guid accountId, string newRole)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null) throw new Exception("Account not found.");

            account.Role = newRole;
            await _accountRepository.UpdateAsync(accountId, account);
        }
    }
}

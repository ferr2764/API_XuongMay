using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Services.Interface;

namespace XuongMay.Services.Service
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            var repository = _unitOfWork.GetRepository<Account>();
            return await repository.GetAllAsync();
        }

        public async Task<Account> GetAccountByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<Account>();
            return await repository.GetByIdAsync(objectId);
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            var repository = _unitOfWork.GetRepository<Account>();
            await repository.InsertAsync(account);
            await _unitOfWork.SaveAsync();
            return account;
        }

        public async Task<Account> UpdateAccountAsync(string id, Account account)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<Account>();
            var existingAccount = await repository.GetByIdAsync(objectId);
            if (existingAccount == null)
                return null;

            // Update các thuộc tính cần thiết
            existingAccount.Name = account.Name;
            existingAccount.Username = account.Username;
            existingAccount.Password = account.Password;
            existingAccount.Role = account.Role;
            existingAccount.Salary = account.Salary;

            repository.Update(existingAccount);
            await _unitOfWork.SaveAsync();

            return existingAccount;
        }

        public async Task<bool> DeleteAccountAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return false;

            var repository = _unitOfWork.GetRepository<Account>();
            var account = await repository.GetByIdAsync(objectId);
            if (account == null)
                return false;

            await repository.DeleteAsync(objectId);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}

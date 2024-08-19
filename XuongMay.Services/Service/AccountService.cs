using MongoDB.Bson;
using MongoDB.Driver;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.AccountModelView;

namespace XuongMay.Services.Service
{
    public class AccountService : IAccountService
    {
        private readonly IMongoCollection<Account> _accounts;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IMongoDatabase database, IUnitOfWork unitOfWork)
        {
            _accounts = database.GetCollection<Account>("Accounts");
            _unitOfWork = unitOfWork;
        }

        public ExposeAccountModelView AccountMapping(Account account)
        {
            ExposeAccountModelView model = new();
            model.Id = account.Id.ToString();
            model.Name = account.Name;
            model.Role = account.Role;
            model.Username = account.Username;
            model.Salary = account.Salary;
            model.Status = account.Status;
            return model;
        }
        //Get account By Id
        public async Task<ExposeAccountModelView> GetAccountByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<Account>();
            var account = await repository.GetByIdAsync(objectId);
            if (account == null)
                return null;

            return AccountMapping(account);
        }

        //Get account By Role
        public async Task<IEnumerable<ExposeAccountModelView>> GetAccountsByRoleAsync(string role)
        {
            var accounts = await _accounts.Find(a => a.Role == role).ToListAsync();
            return accounts.Select(AccountMapping);
        }


        //Get all accounts
        public async Task<IEnumerable<ExposeAccountModelView>> GetAllAccountsAsync(int pageNumber = 1, int pageSize = 5)
        {
            var repository = _unitOfWork.GetRepository<Account>();
            var accounts = await repository.GetAllAsync();

            var pagedAccounts = accounts
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            return pagedAccounts.Select(AccountMapping);
        }


        //Update account
        public async Task<ExposeAccountModelView> UpdateAccountAsync(string id, Account account)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return null;

            var repository = _unitOfWork.GetRepository<Account>();
            var existingAccount = await repository.GetByIdAsync(objectId);
            if (existingAccount == null)
                return null;

            // Update properties
            existingAccount.Name = account.Name;
            existingAccount.Password = account.Password;
            existingAccount.Status = account.Status;
            existingAccount.Salary = account.Salary;

            repository.Update(existingAccount);

            return AccountMapping(existingAccount);
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
            //await _unitOfWork.SaveAsync();

            return true;
        }

        // Update account role
        public async Task UpdateAccountRoleAsync(string accountId, string newRole)
        {
            ObjectId objectId = ObjectId.Parse(accountId);

            // Update the role field of the account
            var updateDefinition = Builders<Account>.Update
                .Set(a => a.Role, newRole);

            // Execute the update
            var result = await _accounts.UpdateOneAsync(a => a.Id == objectId, updateDefinition);

            if (result.MatchedCount == 0)
            {
                throw new Exception("Account not found.");
            }
        }

    }
}
using MongoDB.Bson;
using MongoDB.Driver;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Services.Service
{
    public class AccountService : IAccountService
    {
        private readonly IMongoCollection<Account> _accounts;

        public AccountService(IMongoDatabase database)
        {
            _accounts = database.GetCollection<Account>("Accounts");
        }

        //Get account By Id
        public async Task<Account> GetAccountByIdAsync(string id)
        {
            ObjectId objectId = ObjectId.Parse(id);
            return await _accounts.Find(a => a.Id == objectId).FirstOrDefaultAsync();
        }

        //Get account By Role
        public async Task<IEnumerable<Account>> GetAccountsByRoleAsync(string role)
        {
            return await _accounts.Find(a => a.Role == role).ToListAsync();
        }

        //Get all accounts
        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _accounts.Find(a => true).ToListAsync();
        }

        //Update account
        public async Task UpdateAccountByIdAsync(string id, Account updatedAccount)
        {
            ObjectId objectId = ObjectId.Parse(id);

            // Define the fields to update
            var updateDefinition = Builders<Account>.Update
                .Set(a => a.Name, updatedAccount.Name)
                .Set(a => a.Username, updatedAccount.Username)
                .Set(a => a.Password, updatedAccount.Password)
                .Set(a => a.Role, updatedAccount.Role)
                .Set(a => a.Salary, updatedAccount.Salary);

            // Execute the update
            await _accounts.UpdateOneAsync(a => a.Id == objectId, updateDefinition);
        }
    }
}
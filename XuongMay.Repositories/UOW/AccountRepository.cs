using MongoDB.Bson;
using MongoDB.Driver;
using XuongMay.Contract.Repositories.IUOW;
using XuongMay.Contract.Repositories.Entity;


namespace XuongMay.Repositories.UOW
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMongoCollection<Account> _accounts;

        public AccountRepository(IMongoDatabase database)
        {
            _accounts = database.GetCollection<Account>("Account");
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _accounts.Find(_ => true).ToListAsync();
        }

        public async Task<Account> GetByIdAsync(ObjectId id)
        {
            return await _accounts.Find(account => account.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Account account)
        {
            await _accounts.InsertOneAsync(account);
        }

        public async Task<bool> UpdateAsync(ObjectId id, Account account)
        {
            var result = await _accounts.ReplaceOneAsync(a => a.Id == id, account);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(ObjectId id)
        {
            var result = await _accounts.DeleteOneAsync(a => a.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
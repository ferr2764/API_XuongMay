using MongoDB.Bson;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Contract.Repositories.IUOW
{
    public interface IAccountRepository
    {
        // Get all accounts
        Task<IEnumerable<Account>> GetAllAsync();

        // Get account by Id
        Task<Account> GetByIdAsync(ObjectId id);

        // Create a new account
        Task CreateAsync(Account account);

        // Update an existing account
        Task<bool> UpdateAsync(ObjectId id, Account account);

        // Delete an account by Id
        Task<bool> DeleteAsync(ObjectId id);
    }
}

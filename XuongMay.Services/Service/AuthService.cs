using MongoDB.Driver;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Services.Interface;
using BCrypt.Net;
using System.Threading.Tasks;

namespace XuongMay.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMongoCollection<Account> _accounts;

        public AuthService(IMongoDatabase database)
        {
            _accounts = database.GetCollection<Account>("Accounts");
        }

        public async Task RegisterUserAsync(Account account)
        {
            // Check if the user already exists
            var existingUser = await _accounts.Find(a => a.Username == account.Username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            account.Role = "Customer";
            account.Salary = 1000;
            // Hash the password
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);

            // Insert the user into the MongoDB collection
            await _accounts.InsertOneAsync(account);
        }

        public async Task<Account> AuthenticateUserAsync(string username, string password)
        {
            // Find the user by username
            var user = await _accounts.Find(a => a.Username == username).FirstOrDefaultAsync();
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new Exception("Invalid username or password");
            }

            return user;
        }
    }
}

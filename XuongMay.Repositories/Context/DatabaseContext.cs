using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Repositories
{
    public class DatabaseContext
    {
        private readonly IMongoDatabase _dbcontext;

        public DatabaseContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _dbcontext = client.GetDatabase(configuration["DatabaseName"]);
        }

        public IMongoCollection<Account> Accounts => _dbcontext.GetCollection<Account>("Account");
        public IMongoCollection<Order> Orders => _dbcontext.GetCollection<Order>("Orders");
        public IMongoCollection<OrderDetail> OrderDetails => _dbcontext.GetCollection<OrderDetail>("OrderDetails");
        public IMongoCollection<Category> Categories => _dbcontext.GetCollection<Category>("Categories");
        public IMongoCollection<Product> Products => _dbcontext.GetCollection<Product>("Products");
    }

}
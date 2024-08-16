using MongoDB.Driver;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Services.Interface;
using XuongMay.Repositories.UOW;
using XuongMay.Services;
using XuongMay.Services.Service;

namespace XuongMayBE.API
{
    public static class DependencyInjection
    {
        public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigRoute();
            services.AddMongoDb();
            services.AddInfrastructure(configuration);
            services.AddServices();
        }
        public static void ConfigRoute(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
        }
        public static void AddMongoDb(this IServiceCollection services)
        {
            // Hard-coded MongoDB connection string and database name
            const string connectionString = "mongodb+srv://minhvqse183085:am0C8JUa9aPMSVK2@xuongmay.kuwjh.mongodb.net/";
            const string databaseName = "XuongMay";

            services.AddSingleton<IMongoClient>(sp =>
            {
                return new MongoClient(connectionString);
            });

            services.AddSingleton(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(databaseName);
            });
        }

        public static void AddServices(this IServiceCollection services)
        {
            // Ensure MongoDB services are added
            services.AddMongoDb();

            // Register UnitOfWork with dependencies
            services.AddScoped<IUnitOfWork>(sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                var database = sp.GetRequiredService<IMongoDatabase>();

                return new UnitOfWork(mongoClient, database);
            });

            // Register other services
            services.AddScoped<IUserService, UserService>();
        }

    }
}

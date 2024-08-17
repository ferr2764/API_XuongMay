using MongoDB.Driver;
using XuongMay.Contract.Services.Interface;
using XuongMay.Services;
using XuongMay.Services.Service;

namespace XuongMayBE.API
{
    public static class DependencyInjection
    {
        public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigRoute();
            services.AddMongoDb(configuration);
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
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
{
    var connectionString = configuration["ConnectionStrings:MongoDb"];
    var databaseName = configuration["MongoDbDatabase"];

    services.AddSingleton<IMongoClient>(sp =>
    {
        return new MongoClient(connectionString);
    });

    services.AddSingleton(sp =>
    {
        var client = sp.GetRequiredService<IMongoClient>();
        return client.GetDatabase(databaseName);
    });

    // Register the database name for use by UnitOfWork
    services.AddSingleton(databaseName);
}
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();

        }
    }
}

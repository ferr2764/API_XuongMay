using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using XuongMay.Contract.Services.Interface;
using XuongMay.Services;
using XuongMay.Services.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using XuongMay.Repositories;

namespace XuongMayBE.API
{
    public static class DependencyInjection
    {
        public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigRoute();
            services.AddMongoDb(configuration);
            services.AddInfrastructure(configuration);
            services.AddJwtAuthentication(configuration);
            services.AddSwaggerWithJwtAuth();
            services.AddServices();
        }
        public static void ConfigRoute(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
        }
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("MyCnn"));
            });
        }
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Registers authentication services and sets the default authentication scheme to JWT Bearer
            services.AddAuthentication(options =>
            {
                // Sets the default authentication scheme to JWT Bearer
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Configures the JWT Bearer authentication handler with token validation parameters
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, 
                    ValidateAudience = true, 
                    ValidateLifetime = true, 
                    ValidateIssuerSigningKey = true, 
                    ValidIssuer = configuration["Jwt:Issuer"], 
                    ValidAudience = configuration["Jwt:Audience"], 
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])) // Retrieves the secret key used to sign the token
                };
            });
        }


        public static void AddSwaggerWithJwtAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "XuongMay API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
  {
      {
          new OpenApiSecurityScheme
          {
              Reference = new OpenApiReference
              {
                  Type = ReferenceType.SecurityScheme,
                  Id = "Bearer"
              }
          },
          Array.Empty<string>()
      }
  });
            });
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

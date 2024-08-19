using MongoDB.Driver;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Services.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using XuongMay.ModelViews.AuthModelViews;
using MongoDB.Bson;

namespace XuongMay.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly IMongoCollection<Account> _accounts;
        private readonly IConfiguration _configuration;

        public AuthService(IMongoDatabase database, IConfiguration configuration)
        {
            _accounts = database.GetCollection<Account>("Account");
            _configuration = configuration;
        }

        public async Task RegisterUserAsync(RegisterModelView registerModel)
        {
            // Check if the user already exists
            var existingUser = await _accounts.Find(a => a.Username == registerModel.Username).FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }
            // Create a new Account entity from the RegisterModelView
            var newAccount = new Account
            {
                Id = ObjectId.GenerateNewId(),
                Name = registerModel.Name,
                Username = registerModel.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(registerModel.Password),
                Role = "Customer", 
                Salary = 1000,
                Status = "Available"
            };

            // Insert the user into the MongoDB collection
            await _accounts.InsertOneAsync(newAccount);
        }

        public async Task<(string Token, Account User)> AuthenticateUserAsync(string username, string password)
        {
            // Find the user by username
            var user = await _accounts.Find(a => a.Username == username).FirstOrDefaultAsync();
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new Exception("Invalid username or password");
            }

            // Generate JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return (jwtToken, user);
        }
    }
}

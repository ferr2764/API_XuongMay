using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using XuongMay.Contract.Repositories.Entity;
using XuongMay.Contract.Repositories.Interface;
using XuongMay.Contract.Services.Interface;
using XuongMay.ModelViews.AuthModelViews;

namespace XuongMay.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        public async Task RegisterUserAsync(RegisterModelView registerModel)
        {
            var existingUser = await _accountRepository.GetByPredicateAsync(a => a.Username == registerModel.Username);
            if (existingUser != null) throw new Exception("User already exists");

            var newAccount = new Account
            {
                AccountId = Guid.NewGuid(),
                Name = registerModel.Name,
                Username = registerModel.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(registerModel.Password),
                Role = "Employee",
                Salary = 1000,
                Status = "Available"
            };

            await _accountRepository.InsertAsync(newAccount);
        }

        public async Task<(string Token, Account User)> AuthenticateUserAsync(string username, string password)
        {
            var user = await _accountRepository.GetByPredicateAsync(a => a.Username.ToLower() == username.ToLower());
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new Exception("Invalid username or password");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
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
            return (tokenHandler.WriteToken(token), user);
        }
    }
}

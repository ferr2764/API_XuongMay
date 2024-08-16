using XuongMay.Contract.Repositories.Entity;

namespace XuongMay.Contract.Services.Interface
{
    public interface IAuthService
    {
        Task RegisterUserAsync(Account account);
        Task<Account> AuthenticateUserAsync(string username, string password);
    }
}

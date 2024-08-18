using XuongMay.Contract.Repositories.Entity;
using XuongMay.ModelViews.AuthModelViews;

namespace XuongMay.Contract.Services.Interface
{
    public interface IAuthService
    {
        Task RegisterUserAsync(RegisterModelView registerModel);
        Task<(string Token, Account User)> AuthenticateUserAsync(string username, string password);
    }
}

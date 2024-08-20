using XuongMay.ModelViews.AccountModelView;
using XuongMay.ModelViews.AuthModelViews;

namespace XuongMay.Contract.Services.Interface
{
    public interface IAccountService
    {
        Task<ExposeAccountModelView> GetAccountByIdAsync(string id);
        Task<IEnumerable<ExposeAccountModelView>> GetAccountsByRoleAsync(string role);
        Task<IEnumerable<ExposeAccountModelView>> GetAllAccountsAsync(int pageNumber = 1, int pageSize = 5);
        Task<ExposeAccountModelView> UpdateAccountAsync(string id, UpdateAccountModelVIew account);
        Task<bool> DeleteAccountAsync(string id);
        Task UpdateAccountRoleAsync(string accountId, string newRole);
    }
}
using XuongMay.ModelViews.AccountModelView;
using XuongMay.ModelViews.AuthModelViews;

namespace XuongMay.Contract.Services.Interface
{
    public interface IAccountService
    {
        Task<ExposeAccountModelView> GetAccountByIdAsync(Guid id);
        Task<IEnumerable<ExposeAccountModelView>> GetAccountsByRoleAsync(string role);
        Task<IEnumerable<ExposeAccountModelView>> GetAllAccountsAsync(int pageNumber = 1, int pageSize = 5);
        Task<ExposeAccountModelView> UpdateAccountAsync(Guid id, UpdateAccountModelView account);
        Task<bool> DeleteAccountAsync(Guid id);
        Task UpdateAccountRoleAsync(Guid accountId, string newRole);
    }
}

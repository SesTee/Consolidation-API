using CoreLib.Models.Account.Request;
using CoreLib.Models.Account.Response;
using System.Threading.Tasks;

namespace CoreLib.Interfaces
{
    public interface IAccountInterface
    {
        Task<AccountListResponse> GetAllAccountsAsync();
        Task<AccountDetailResponse> GetAccountDetailAsync(string acctnum);
        Task<AccountBalanceResponse> GetAccountBalanceAsync(string acctnum);
        Task<AccountResponse> AddAccountAsync(AddAccount account);
        Task<AccountResponse> EditAccountAsync(EditAccount account);
    }
}
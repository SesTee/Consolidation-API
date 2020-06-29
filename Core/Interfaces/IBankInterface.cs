using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLib.Models.Bank.Request;
using CoreLib.Models.Bank.Response;

namespace CoreLib.Interfaces
{
    public interface IBankInterface
    {
        Task<BankListResponse> GetAllBanksAsync();
        Task<BankDetailResponse> GetBankAsync(string bankcode);
        Task<BankAccountListResponse> GetBankAccountsAsync(string bankcode);
        Task<BankBalanceResponse> GetBankBalanceAsync(string bankcode);
        Task<BankResponse> AddBankAsync(AddBank bank);
        Task<BankResponse> EditBankAsync(ModifyBank bank);
    }
}
using API.Controllers;
using DomainClassLib.Models.API.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainClassLib.Interfaces
{
    public interface IBankInterface
    {
        Task<List<BankResponse>> GetAllBanksAsync();
        Task<BankDetailResponse> GetBankDetailAsync(string bankcode);
        Task<BankAccountListResponse> GetBankAccountsAsync(string bankcode);
        Task<BankBalanceResponse> GetBankBalanceAsync(string bankcode);
        Task<BankResponse> AddBankAsync(CreateBank bank);
        Task<BankResponse> EditBankAsync(ModifyBank bank);
    }
}
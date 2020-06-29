using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreLib.Interfaces;
using CoreLib.Models.Bank.Request;
using CoreLib.Models.Bank.Response;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : BaseController
    {

        private readonly IBankInterface _bank;

        public BankController(IBankInterface bankInterface)
        {
            _bank = bankInterface;
        }

        [HttpGet("GetAllBanks")]
        public async Task<BankListResponse> GetAllBanks() => await _bank.GetAllBanksAsync();

        [HttpGet("GetBank")]
        public async Task<BankDetailResponse> GetBank(string bankcode) => await _bank.GetBankAsync(bankcode);

        [HttpGet("GetBankAccounts")]
        public async Task<BankAccountListResponse> GetBankAccounts(string bankcode) => await _bank.GetBankAccountsAsync(bankcode);

        [HttpGet("GetBankBalance")]
        public async Task<BankBalanceResponse> GetBankBalance(string bankcode) => await _bank.GetBankBalanceAsync(bankcode);

        [HttpPost("Add")]
        public async Task<BankResponse> Add(AddBank bank) => await _bank.AddBankAsync(bank);

        [HttpPost("Modify")]
        public async Task<BankResponse> Modify(ModifyBank bank) => await _bank.EditBankAsync(bank);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CoreLib.Interfaces;
using CoreLib.Models.Account.Request;
using CoreLib.Models.Account.Response;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {

        private readonly IAccountInterface _account;

        public AccountController(IAccountInterface accountInterface)
        {
            _account = accountInterface;
        }

        [HttpGet("GetAllAccounts")]
        public async Task<AccountListResponse> GetAllAccounts() => await _account.GetAllAccountsAsync();

        [HttpGet("GetAccountDetail")]
        public async Task<AccountDetailResponse> GetAccountDetail(string acctnum, string bankcode) => await _account.GetAccountDetailAsync(acctnum);


        [HttpGet("GetAccountBalance")]
        public async Task<AccountBalanceResponse> GetAccountBalance(string acctnum, string bankcode) => await _account.GetAccountBalanceAsync(acctnum);

        [HttpPost("Add")]
        public async Task<AccountResponse> Add(AddAccount account) => await _account.AddAccountAsync(account);

        [HttpPost("Modify")]
        public async Task<AccountResponse> Modify(EditAccount account) => await _account.EditAccountAsync(account);
    }
}

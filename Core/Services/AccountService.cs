using CommonLib;
using CoreLib.Interfaces;
using CoreLib.Models.Account;
using CoreLib.Models.Account.Request;
using CoreLib.Models.Account.Response;
using DomainClassLib.Data.Entities;
using DomainClassLib.Data.Repositories;
using LoggerClassLib.Middleware;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLib.Services
{
    public class AccountService : IAccountInterface
    {
        private readonly AccountRepository _accountRepo = new AccountRepository();

        public async Task<AccountResponse> AddAccountAsync(AddAccount account)
        {

            AccountEntity accountEntity = JsonConvert.DeserializeObject<AccountEntity>(JsonConvert.SerializeObject(account));

            var resp = await _accountRepo.AddAccountAsync(accountEntity);

            if (resp == 1)
                return new AccountResponse { RequestID = account.RequestID, StatusCode = "00", Message = "Successfully Added" };
            else
                throw new BadRequestException(new GeneralResponse { code = "400", message = "Failed To Add Account" });

        }

        public async Task<AccountResponse> EditAccountAsync(EditAccount account)
        {
            AccountEntity accountEntity = JsonConvert.DeserializeObject<AccountEntity>(JsonConvert.SerializeObject(account));

            var resp = await _accountRepo.EditAccountAsync(accountEntity);

            if (resp == 1)
                return new AccountResponse { RequestID = account.RequestID, StatusCode = "00", Message = "Successfully Changed" };
            else
                throw new BadRequestException(new GeneralResponse { code = "400", message = "Failed To Modify Account" });
        }

        public async Task<AccountListResponse> GetAllAccountsAsync()
        {
            string requestID = Guid.NewGuid().ToString();
            var resp = await _accountRepo.GetAllAccountsAsync();
            List<AccountDet> accountDets = JsonConvert.DeserializeObject<List<AccountDet>>(JsonConvert.SerializeObject(resp));

            return new AccountListResponse { RequestID = requestID, StatusCode = "00", Accounts = accountDets, Message = resp.Count + " Record(s) returned" };
        }

        public async Task<AccountDetailResponse> GetAccountAsync(string accountcode)
        {
            string requestID = Guid.NewGuid().ToString();
            var resp = await _accountRepo.GetAccountAsync(accountcode);

            if (resp is null)
                throw new BadRequestException(new GeneralResponse { code = "400", message = "Account not found" });

            AccountDet accountDet = JsonConvert.DeserializeObject<AccountDet>(JsonConvert.SerializeObject(resp));

            return new AccountDetailResponse { RequestID = requestID, StatusCode = "00", Account = accountDet, Message = "OK" };
        }

        public Task<AccountBalanceResponse> GetAccountBalanceAsync(string acctnum)
        {
            throw new NotImplementedException();
        }

        public Task<AccountDetailResponse> GetAccountDetailAsync(string acctnum)
        {
            throw new NotImplementedException();
        }

    }
}

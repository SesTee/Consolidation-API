using CommonLib;
using CoreLib.Interfaces;
using CoreLib.Models.Bank;
using CoreLib.Models.Bank.Request;
using CoreLib.Models.Bank.Response;
using DomainClassLib.Data.Entities;
using DomainClassLib.Data.Repositories;
using LoggerClassLib.Middleware;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLib.Services
{

    public class BankService : IBankInterface
    {
        private readonly BankRepository _bankRepo = new DomainClassLib.Data.Repositories.BankRepository();

        public async Task<BankResponse> AddBankAsync(AddBank bank)
        {

            BankEntity bankEntity = JsonConvert.DeserializeObject<BankEntity>(JsonConvert.SerializeObject(bank));

            var resp = await _bankRepo.AddBankAsync(bankEntity);

            if (resp == 1)
                return new BankResponse { RequestID = bank.RequestID, StatusCode = "00", Message = "Successfully Added" };
            else
                throw new BadRequestException(new GeneralResponse { code = "400", message = "Failed To Add Bank" });

        }

        public async Task<BankResponse> EditBankAsync(ModifyBank bank)
        {
            BankEntity bankEntity = JsonConvert.DeserializeObject<BankEntity>(JsonConvert.SerializeObject(bank));

            var resp = await _bankRepo.EditBankAsync(bankEntity);

            if (resp == 1)
                return new BankResponse { RequestID = bank.RequestID, StatusCode = "00", Message = "Successfully Changed" };
            else
                throw new BadRequestException(new GeneralResponse { code = "400", message = "Failed To Modify Bank" });
        }

        public async Task<BankListResponse> GetAllBanksAsync()
        {
            string requestID  = Guid.NewGuid().ToString();
            var resp = await _bankRepo.GetAllBanksAsync();
            List<BankDet> bankDets = JsonConvert.DeserializeObject<List<BankDet>>(JsonConvert.SerializeObject(resp));

            return new BankListResponse { RequestID = requestID, StatusCode = "00", Banks = bankDets, Message = resp.Count + " Record(s) returned" };

        }

        public async Task<BankDetailResponse> GetBankAsync(string bankcode)
        {
            string requestID = Guid.NewGuid().ToString();
            var resp = await _bankRepo.GetBankAsync(bankcode);

            if (resp is null)
                throw new BadRequestException(new GeneralResponse { code = "400", message = "Bank not found" });

            BankDet bankDet = JsonConvert.DeserializeObject<BankDet>(JsonConvert.SerializeObject(resp));

            return new BankDetailResponse { RequestID = requestID, StatusCode = "00", Bank = bankDet, Message = "OK" };

        }

        public Task<BankAccountListResponse> GetBankAccountsAsync(string bankcode)
        {
            throw new NotImplementedException();
        }

        public Task<BankBalanceResponse> GetBankBalanceAsync(string bankcode)
        {
            throw new NotImplementedException();
        }

    }
}

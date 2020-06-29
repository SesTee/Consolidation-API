using System.Collections.Generic;

namespace CoreLib.Models.Account.Response
{
    public class AccountListResponse : BaseResponse
    {
        public List<AccountDet> Accounts { get; internal set; }
    }
}
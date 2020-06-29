using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.Models.Bank.Response
{
    public class BankListResponse: BaseResponse
    {
        public List<BankDet> Banks { get; set; }
    }
}

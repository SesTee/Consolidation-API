using CoreLib.Interfaces;
using CoreLib.Models.Statement.Request;
using CoreLib.Models.Statement.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Services
{
    public class StatementService : IStatementInterface
    {
        public Task<StatementResponse> GetAccountStatementAsync(StatementRequest statementRequest)
        {
            throw new NotImplementedException();
        }
    }
}

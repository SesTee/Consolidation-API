using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLib.Models.Statement.Request;
using CoreLib.Models.Statement.Response;

namespace CoreLib.Interfaces
{
    public interface IStatementInterface
    {
        Task<StatementResponse> GetAccountStatementAsync(StatementRequest statementRequest);
    }
}
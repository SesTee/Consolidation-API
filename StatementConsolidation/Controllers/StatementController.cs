using CoreLib.Interfaces;
using CoreLib.Models.Statement.Request;
using CoreLib.Models.Statement.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatementController : BaseController
    {

        private readonly IStatementInterface _statement;

        //private readonly IHttpContextAccessor _ctx;

        public StatementController(IStatementInterface statementInterface)
        {
            //_ctx = ctx;
            _statement = statementInterface;
        }

        [HttpGet("GetAccountStatement")]
        // [LogUsage("GetAllStatements")]
        public async Task<StatementResponse> GetAccountStatement(StatementRequest statementRequest)
        {
            return await _statement.GetAccountStatementAsync(statementRequest);
        }

    }
}

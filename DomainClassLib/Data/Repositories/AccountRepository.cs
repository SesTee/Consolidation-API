using CommonLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DomainClassLib.Data.Entities;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace DomainClassLib.Data.Repositories
{
    public class AccountRepository
    {
        static readonly string connstring = Utility.AppConfiguration().GetSection("ConnectionStrings").GetSection("StatementDB").Value;


        public async Task<int> AddAccountAsync(AccountEntity account)
        {
            string sql = Utility.AppConfiguration().GetSection("StmtDBQueries").GetSection("addaccount").Value;
            int rows;

            using (var connection = new SqlConnection(connstring))
            {
                rows = await connection.ExecuteAsync(sql,account);
            }
            return rows;
        }

        public async Task<int> EditAccountAsync(AccountEntity account)
        {

            string sql = Utility.AppConfiguration().GetSection("StmtDBQueries").GetSection("editaccount").Value;
            int rows;

            using (var connection = new SqlConnection(connstring))
            {
                rows = await connection.ExecuteAsync(sql, account);
            }
            return rows;
        }

        public async Task<List<AccountEntity>> GetAllAccountsAsync()
        {
            List<AccountEntity> accounts = new List<AccountEntity>();
            string sql = Utility.AppConfiguration().GetSection("StmtDBQueries").GetSection("getallaccounts").Value;

            using (var connection = new SqlConnection(connstring))
            {
                var resp = await connection.QueryAsync<AccountEntity>(sql);
                accounts = resp.ToList();
            }

            return accounts;
        }

        public async Task<AccountEntity> GetAccountAsync(string accountcode)
        {
            AccountEntity account = new AccountEntity();
            string sql = Utility.AppConfiguration().GetSection("StmtDBQueries").GetSection("getaccount").Value;

            using (var connection = new SqlConnection(connstring))
            {
                var resp = await connection.QueryAsync<AccountEntity>(sql, new { accountcode });
                account = resp.FirstOrDefault();
            }

            return account;
        }
    }
}

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
    public class BankRepository
    {
        static readonly string connstring = Utility.AppConfiguration().GetSection("ConnectionStrings").GetSection("StatementDB").Value;


        public async Task<int> AddBankAsync(BankEntity bank)
        {
            string sql = Utility.AppConfiguration().GetSection("StmtDBQueries").GetSection("addbank").Value;
            int rows;

            using (var connection = new SqlConnection(connstring))
            {
                rows = await connection.ExecuteAsync(sql,bank);
            }
            return rows;
        }

        public async Task<int> EditBankAsync(BankEntity bank)
        {

            string sql = Utility.AppConfiguration().GetSection("StmtDBQueries").GetSection("editbank").Value;
            int rows;

            using (var connection = new SqlConnection(connstring))
            {
                rows = await connection.ExecuteAsync(sql, bank);
            }
            return rows;
        }

        public async Task<List<BankEntity>> GetAllBanksAsync()
        {
            List<BankEntity> banks = new List<BankEntity>();
            string sql = Utility.AppConfiguration().GetSection("StmtDBQueries").GetSection("getallbanks").Value;

            using (var connection = new SqlConnection(connstring))
            {
                var resp = await connection.QueryAsync<BankEntity>(sql);
                banks = resp.ToList();
            }

            return banks;
        }

        public async Task<BankEntity> GetBankAsync(string bankcode)
        {
            BankEntity bank = new BankEntity();
            string sql = Utility.AppConfiguration().GetSection("StmtDBQueries").GetSection("getbank").Value;

            using (var connection = new SqlConnection(connstring))
            {
                var resp = await connection.QueryAsync<BankEntity>(sql, new { bankcode });
                bank = resp.FirstOrDefault();
            }

            return bank;
        }
    }
}

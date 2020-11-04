using TransactionLibrary.API.Entities;
using System;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using TransactionService.API.Entities;

namespace TransactionLibrary.API.Services
{
    public class TransactionLibraryRepository : ITransactionLibraryRepository, IDisposable
    {

        private readonly string connstring;
        private IDbConnection Connection => new SqlConnection(connstring);
        public TransactionLibraryRepository()
        {

            //  connstring = "Server=192.168.0.164;Database=maecbsdb;user=SA;Password=TCSuser1123;Trusted_Connection=True;";
            connstring = "Server=192.168.0.164;Database=maecbsdb;user=SA;Password=TCSuser1123;";
        }

        public bool TransactionDetails(Transaction req)
        {
            using (var c = Connection)
            {
                try
                {
                    c.Open();
                    var p = new DynamicParameters();
                    p.Add("accountNumber", req.AccountIdentifier, DbType.String, ParameterDirection.Input);

                    // Select from Accounts Table
                    string selectQuery = "select [available_balance] from [tb_mae_mort_account] where account_identifier= @accountNumber";
                    var selectedAccounts = c.Query<double>(selectQuery, p).FirstOrDefault();

                    var p2 = new DynamicParameters();

                    p2.Add("accountNumber", req.AccountIdentifier, DbType.String, ParameterDirection.Input);
                    p2.Add("transactionAmount", req.TransactionAmount, DbType.Double, ParameterDirection.Input);
                    p2.Add("transactionDate", req.TransactionDate, DbType.DateTime, ParameterDirection.Input);
                    p2.Add("transactionTowards", req.TransactionTowards, DbType.String, ParameterDirection.Input);
                    p2.Add("transactionType", req.TransactionType, DbType.String, ParameterDirection.Input);
                    p2.Add("availableBalance", selectedAccounts, DbType.Double, ParameterDirection.Input);
                    //Update Transaction Table
                    //string updateQuery = "update tb_mae_mort_account_trans set [account_identifier]=@accountNumber, [available_balance] = @availableBalance" +
                    //     "[transaction_type] =@transactionType,[transaction_amount] = @transactionAmount, [transaction_towards]= @transactionTowards" +
                    //     " [transaction_date] =@transactionDate where account_identifier= @accountNumber";
                    //
                    string query = " insert into tb_mae_mort_account_trans (account_identifier,transaction_type,available_balance,transaction_amount,transaction_towards,transaction_date) values(@accountNumber, @transactionType, @availableBalance, @transactionAmount, @transactionTowards, @transactionDate)";

                    var updatedAccounts = c.Query(query, p2);

                }catch(Exception e)
                {

                }
                //p.Add("accountNumber", req.AccountIdentifier, DbType.String, ParameterDirection.Input);
                ////p.Add("transactionAmount", req.TransactionAmount, DbType.Double, ParameterDirection.Input);
                ////p.Add("transactionDate", req.TransactionDate, DbType.DateTime, ParameterDirection.Input);
                ////p.Add("transactionTowards", req.TransactionTowards, DbType.String, ParameterDirection.Input);
                ////p.Add("transactionType", req.TransactionType, DbType.String, ParameterDirection.Input);
                ////p.Add("availableBalance", req.AvailableBalance, DbType.Double, ParameterDirection.Input);

                //// Select from Accounts Table
                //string selectQuery = "select account_identifier,amount from tb_mae_account where account_identifier= @accountNumber";
                //var selectedAccounts = c.Query<AccountDetails>(selectQuery, p);


                //var p2 = new DynamicParameters();

                //foreach (var account in selectedAccounts)
                //{
                //    p2.Add("accountNumber", account.account_identifier, DbType.String, ParameterDirection.Input);
                //    p2.Add("transactionAmount", account.amount, DbType.Double, ParameterDirection.Input);
                //    p2.Add("transactionDate", req.TransactionDate, DbType.DateTime, ParameterDirection.Input);
                //    p2.Add("transactionTowards", req.TransactionTowards, DbType.String, ParameterDirection.Input);
                //    p2.Add("transactionType", req.TransactionType, DbType.String, ParameterDirection.Input);
                //    //p2.Add("availableBalance", req.AvailableBalance, DbType.Double, ParameterDirection.Input);
                //    //Update Transaction Table
                //    //string updateQuery = "update tb_mae_mort_account_trans set [account_identifier]=@accountNumber, [available_balance] = @availableBalance" +
                //    //     "[transaction_type] =@transactionType,[transaction_amount] = @transactionAmount, [transaction_towards]= @transactionTowards" +
                //    //     " [transaction_date] =@transactionDate where account_identifier= @accountNumber";
                //    //
                //    string query = " insert into tb_mae_mort_account_trans (account_identifier,transaction_type,transaction_amount,transaction_towards,transaction_date) values(@accountNumber, @transactionType, @transactionAmount, @transactionTowards, @transactionDate)";

                //    var updatedAccounts = c.Query(query, p2);
                //}            


                c.Close();
                return true;
            }           
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }
    }
}

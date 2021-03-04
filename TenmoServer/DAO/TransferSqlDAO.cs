using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDAO : ITransferDAO
    {
        private readonly string connectionString;

        public TransferSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Transfer TransferSend(Transfer transfer)
        {
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    //create new send transfer
                    SqlCommand cmd = new SqlCommand("Insert into transfers values((select transfer_type_id from transfer_types where transfer_type_desc = 'send'),(select transfer_status_id from transfer_statuses where transfer_status_desc = 'approved'), @fromId, @toID, @amount);", conn);
                    cmd.Parameters.AddWithValue("@fromId", transfer.Account_From);
                    cmd.Parameters.AddWithValue("@toId", transfer.Account_To);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    int rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                    //update balance sending account
                    cmd = new SqlCommand($"update accounts set balance = (balance - @amount) where user_id = @fromId",conn);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);
                    cmd.Parameters.AddWithValue("@fromId", transfer.Account_From);
                    cmd.ExecuteNonQuery();
               
                    //update balance recieving account
                    cmd = new SqlCommand($"update accounts set balance = (balance + @amount) where user_id = @toId",conn);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);
                    cmd.Parameters.AddWithValue("@toId", transfer.Account_To);
                    cmd.ExecuteNonQuery();
                }
                return transfer;
            }
            catch (SqlException)
            {
                throw;
            }
        }
    }
}

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

        public List<Transfer> GetTransfers(int userId)
        {
            List<Transfer> transfers = new List<Transfer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sqlText = "select transfer_id, type.transfer_type_desc, status.transfer_status_desc, " +
                        "userfrom.username as userfrom, userfrom.user_id as userfromid, " +
                    "userto.username as userto, userto.user_id as usertoid, amount from transfers " +
                    "join transfer_types as type on type.transfer_type_id = transfers.transfer_type_id " +
                    "join transfer_statuses as status on status.transfer_status_id = transfers.transfer_status_id " +
                    "join accounts as accountFrom on transfers.account_from = accountFrom.account_id " +
                    "join accounts as accountTo on transfers.account_to = accountTo.account_id " +
                    "join users as userfrom on accountFrom.user_id = userfrom.user_id " +
                    "join users as userto on accountTo.user_id = userto.user_id " +
                    "where (userfrom.user_id = (select user_id from users where user_id = @userId) " +
                    "or userto.user_id = (select user_id from users where user_id = @userId));";

                    SqlCommand cmd = new SqlCommand(sqlText, conn);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Transfer t = GetTransferFromReader(reader);
                            transfers.Add(t);
                        }

                    }

                }
            }
            catch (SqlException e)
            {
                
            }
            return transfers;
        }
        public Transfer GetTransfer(int userId, int transferId)
        {
            Transfer transfer = new Transfer();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sqlText = "select transfer_id, type.transfer_type_desc, status.transfer_status_desc, " +
                        "userfrom.username as userfrom, userfrom.user_id as userfromid, " +
                    "userto.username as userto, userto.user_id as usertoid, amount from transfers " +
                    "join transfer_types as type on type.transfer_type_id = transfers.transfer_type_id " +
                    "join transfer_statuses as status on status.transfer_status_id = transfers.transfer_status_id " +
                    "join accounts as accountFrom on transfers.account_from = accountFrom.account_id " +
                    "join accounts as accountTo on transfers.account_to = accountTo.account_id " +
                    "join users as userfrom on accountFrom.user_id = userfrom.user_id " +
                    "join users as userto on accountTo.user_id = userto.user_id " +
                    "where (userfrom.user_id = (select user_id from users where user_id = @userId and transfer_id = @transferId) " +
                    "or userto.user_id = (select user_id from users where user_id = @userId and transfer_id = @transferId));";

                    SqlCommand cmd = new SqlCommand(sqlText, conn);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@transferId", transferId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                           transfer = GetTransferFromReader(reader);
                            
                        }

                    }

                }
            }
            catch (SqlException e)
            {

            }
            return transfer;
        }

        public Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            transfer.TransferId = Convert.ToInt32(reader["transfer_id"]);
            transfer.Type = Convert.ToString(reader["transfer_type_desc"]);
            transfer.Status = Convert.ToString(reader["transfer_status_desc"]);
            transfer.UserFrom = Convert.ToString(reader["userfrom"]);
            transfer.Account_From = Convert.ToInt32(reader["userfromid"]);
            transfer.UserTo = Convert.ToString(reader["userto"]);
            transfer.Account_To = Convert.ToInt32(reader["usertoid"]);
            transfer.Amount = Convert.ToInt32(reader["amount"]);
            return transfer;
        }
    }
}

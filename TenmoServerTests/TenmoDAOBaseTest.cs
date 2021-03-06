using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;
using TenmoServer.Security;
using TenmoServer.Security.Models;

namespace TenmoServerTests
{
    [TestClass]
    public class TenmoDAOBaseTest
    {
        protected TransactionScope transaction { get; set; }
        //may need to change this if the string isn't right
        protected string connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = Tenmo;Integrated Security=true";
        //set up test variables here
        protected string testUser1 = "batman";

        protected string testPW1 = "batman";

        protected string testUser2 = "robin";

        protected string testPW2 = "robin";
        protected int testId1 { get; set; }
        protected int testId2 { get; set; }
        protected int testTransId { get; set; }

        protected int testAcc1Id { get; set; }
        protected int testAcc2Id { get; set; }
        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            //set up sql statements here that we will need for our tests
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // check to see if user 1 is already in the database
                    string sqlSelect = $"select count(*) from users where username='{testUser1}'";
                    SqlCommand cmd = new SqlCommand(sqlSelect, connection);
                    int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                    if (userCount == 0)
                    {
                        IPasswordHasher passwordHasher = new PasswordHasher();
                        PasswordHash hash = passwordHasher.ComputeHash(testPW1);
                        string insert = $"insert into users values('{testUser1}', '{hash.Password}', '{hash.Salt}');select scope_Identity();";
                        cmd = new SqlCommand(insert, connection);
                        testId1 = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    // check to see if user 2 is already in the database
                    sqlSelect = $"select count(*) from users where username='{testUser2}'";
                    cmd = new SqlCommand(sqlSelect, connection);
                    userCount = Convert.ToInt32(cmd.ExecuteScalar());
                    if (userCount == 0)
                    {
                        IPasswordHasher passwordHasher = new PasswordHasher();
                        PasswordHash hash = passwordHasher.ComputeHash(testPW2);
                        string insert = $"insert into users values('{testUser2}', '{hash.Password}', '{hash.Salt}');select scope_Identity();";
                        cmd = new SqlCommand(insert, connection);
                        testId2 = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    string sqlAccountInsert1 = $"insert into accounts values({testId1}, 1000.00);select scope_Identity();";
                    cmd = new SqlCommand(sqlAccountInsert1,connection);
                    testAcc1Id = Convert.ToInt32(cmd.ExecuteScalar());
                    
                    string sqlAccountInsert2 = $"insert into accounts values({testId2}, 1000.00);select scope_Identity();";
                    cmd = new SqlCommand(sqlAccountInsert2, connection);
                    testAcc2Id = Convert.ToInt32(cmd.ExecuteScalar());

                    //create a transfer for testing
                    string sqlTransferInsert = $"insert into transfers VALUES(2, 2, {testAcc1Id}, {testAcc2Id}, 5.00);select scope_Identity();";
                    cmd = new SqlCommand(sqlTransferInsert, connection);
                    testTransId = Convert.ToInt32(cmd.ExecuteScalar());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }
        // incase we need this for the tests to check rows affected.
        protected int GetRowCount(string table)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand($"select count(*) from {table}", conn);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count;
                }
            }
            catch (SqlException)
            {

                throw;
            }
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;


namespace TenmoServerTests
{
    [TestClass]
    public class TenmoDAOBaseTest
    {
        protected TransactionScope transaction { get; set; }
        //may need to change this if the string isn't right
        protected string connectionString = @"Data Source =.\SQLEXPRESS;Initial Catalog = TenmoDB;Integrated Security=true";
        //set up test variables here

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            //set up sql statements here that we will need for our tests
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

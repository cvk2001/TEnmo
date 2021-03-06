using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServerTests
{
    [TestClass]
    public class AccountSqlDAOTests : TenmoDAOBaseTest
    {
        [TestMethod]
        public void GetAccountByUserHappyPath()
        {
            AccountSqlDAO dao = new AccountSqlDAO(connectionString);

            Account account = dao.GetAccount(testId1);

            Assert.IsTrue(account != null);
        }
    }
}

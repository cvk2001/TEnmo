using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServerTests
{
    [TestClass]
    public class TransferSqlDAOTests : TenmoDAOBaseTest
    {
        [TestMethod]
        public void GetTransfersByIdHappyPath()
        {
            TransferSqlDAO dao = new TransferSqlDAO(connectionString);

            List<Transfer> transferList = dao.GetTransfers(testId1);

            Assert.IsTrue(transferList.Count > 0);
        }
        [TestMethod]
        public void GetTransferByUserAndTransferIdHAppyPath()
        {
            TransferSqlDAO dao = new TransferSqlDAO(connectionString);

            Transfer transfer = dao.GetTransfer(testId1,testTransId);

            Assert.IsTrue(transfer != null);
        }
        [TestMethod]
        public void SendTransferHappyPath()
        {
            // Arrange
            Transfer transfer= new Transfer();
            transfer.Type = "send";
            transfer.Status="approved";
            transfer.Account_From = testAcc1Id;
            transfer.Account_To = testAcc2Id;
            transfer.Amount = 5.00M;

            int startingTransCount = GetRowCount("transfers");

            TransferSqlDAO dao = new TransferSqlDAO(connectionString);
            // Act
            dao.CreateTransfer(transfer);

            int endingTransCount = GetRowCount("transfers");

            //Assert
            Assert.AreNotEqual(startingTransCount, endingTransCount);
        }
    }
}

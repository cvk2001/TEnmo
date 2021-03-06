using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServerTests
{
    [TestClass]
    public class UserSqlDAOTests : TenmoDAOBaseTest
    {
        [TestMethod]
        public void GetUserByUsernameHappyPath()
        {
            UserSqlDAO dao = new UserSqlDAO(connectionString);

            User user = dao.GetUser(testUser1);

            Assert.IsTrue(user != null);
        }
        [TestMethod]
        public void GetAllUsersHappyPath()
        {
            UserSqlDAO dao = new UserSqlDAO(connectionString);

            List<User> userList = dao.GetUsers();

            Assert.IsTrue(userList.Count > 0);
        }
        [TestMethod]
        public void AddUserHappyPath()
        {
            // Arrange
            string username = "orange";
            string password = "julius";

            int startingUsersCount = GetRowCount("users");

            UserSqlDAO dao = new UserSqlDAO(connectionString);
            
            // Act
            dao.AddUser(username,password);

            int endingUsersCount = GetRowCount("users");

            //Assert
            Assert.AreNotEqual(startingUsersCount, endingUsersCount);
        }
    }
}

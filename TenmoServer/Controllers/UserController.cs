using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserDAO UserDAO;
        public UserController(IUserDAO userDAO)
        {
            UserDAO = userDAO;
        }

        [HttpGet]
        public List<User> GetUsers()
        {
            List<User> userList = UserDAO.GetUsers();
            return userList;
        }
    }
}

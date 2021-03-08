using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountDAO AccountDAO;
        public AccountsController(IAccountDAO accountDAO)
        {
            AccountDAO = accountDAO;
        }

        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {
            Account account = AccountDAO.GetAccount(id);
            if (account != null)
            {
                return Ok(account);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("{id}")]
        public ActionResult<Account> UpdateBalance(int id, Account account)
        {
            Account existingAccount = AccountDAO.GetAccount(id);
            if (existingAccount == null)
            {
                return NotFound("Account not found");
            }
            Account updatedBalance = AccountDAO.UpdateBalances(id, account);
            return Ok(updatedBalance);
        }
    }
}

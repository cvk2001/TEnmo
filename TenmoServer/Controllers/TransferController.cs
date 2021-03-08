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
    public class TransferController : ControllerBase
    {
        private readonly ITransferDAO TransferDAO;
        public TransferController(ITransferDAO transferDAO)
        {
            TransferDAO = transferDAO;
        }
        [HttpPost]
        public ActionResult<Transfer> CreateTransfer(Transfer transfer)
        {
            Transfer returnTransfer = TransferDAO.CreateTransfer(transfer);
            return Created($"/transfers/{returnTransfer}", returnTransfer);
        }
        //[HttpPut("{id}")]
        //public ActionResult<Account> UpdateBalance(int transferId, Transfer transfer)
        //{
        //    int userId = Int32.Parse(User.FindFirst("sub").Value);
        //    //Transfer existingTransfer = TransferDAO.GetTransfer(userId,transferId);
        //    //if (existingAccount == null)
        //    //{
        //    //    return NotFound("Account not found");
        //    //}
        //    Account updatedBalance = AccountDAO.UpdateBalances(id, amount, account);
        //    return Ok(updatedBalance);
        //}

        //public ActionResult<Account> UpdateBalances(int)
        //{
        //    //need to find a way to bring id in.  Might need to make the dao only update one balance at a time and 
        //    //have the api service on the client side trigger the updates based off of who it is updateing and pass that id over.
        //    //For example, when we do the send, first the apiservice creates a put to update the senders balance and triggers the httpput with that id,
        //    //then the apiservice creates a different put to update the receivers balance and triggers the httpput with that id.  
        //    //Less code this way and also reusable but will require us to add the put to the apiservice and edit our menu to call appropriate methods.
        //    //also if we do it this way, the methods in this process will return a balance and probably be focused on the accountsdao and not transfer.  
        //    //therefore the method for updating balance that is currently in transferDAO might need to go in accountdao, 
        //    //and instead of bringing in a transfer it will probably have to bring in an id and the amount.
        //}

        [HttpGet("{id}")]
        public ActionResult<List<Transfer>> GetTransfers(int id)
        {

            List<Transfer> returnList = TransferDAO.GetTransfers(id);
            if (returnList != null)
            {
                return Ok(returnList);
            }
            else
            {
                return NotFound();
            }

        }
        [HttpGet("{id}/{transferId}")]
        public ActionResult<Transfer> GetTransfer(int id, int transferId)
        {
            Transfer transfer = TransferDAO.GetTransfer(id, transferId);
            if (transfer != null)
            {
                return Ok(transfer);
            }
            else
            {
                return NotFound();
            }
        }
    }
}

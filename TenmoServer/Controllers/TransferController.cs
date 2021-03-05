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
        public ActionResult<Transfer> CreateTransferSend(Transfer transfer)
        {
            Transfer returnTransfer = TransferDAO.TransferSend(transfer);
            return Created($"/transfers/{returnTransfer}", returnTransfer);
        }
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

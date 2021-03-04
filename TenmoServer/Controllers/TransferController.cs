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
    }
}

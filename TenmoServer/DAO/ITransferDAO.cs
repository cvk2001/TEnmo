using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        Transfer TransferSend(Transfer transfer);

        List<Transfer> GetTransfers(int id);

        Transfer GetTransfer(int id, int transferId);
    }
}

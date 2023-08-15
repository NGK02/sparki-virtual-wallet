using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Dto.ViewModels.TransferViewModels
{
    public class GetTransferViewModel
    {
        public bool HasCardSender { get; set; }

        public decimal Amount { get; set; }

        public string Card { get; set; }

        public string Currency { get; set; }

        public string Date { get; set; }

        public string Username { get; set; }
    }
}
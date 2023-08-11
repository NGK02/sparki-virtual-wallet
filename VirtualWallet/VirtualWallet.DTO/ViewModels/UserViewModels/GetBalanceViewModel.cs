using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Dto.ViewModels.UserViewModels
{
    public class GetBalanceViewModel
    {
        //public int WalletId { get; set; }
        //public Wallet Wallet { get; set; }

        public Currency Currency { get; set; }

        public decimal Amount { get; set; }
    }
}

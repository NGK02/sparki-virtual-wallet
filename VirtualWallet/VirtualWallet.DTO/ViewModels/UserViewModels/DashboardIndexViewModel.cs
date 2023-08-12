using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.ViewModels.CardViewModels;

namespace VirtualWallet.Dto.ViewModels.UserViewModels
{
    public class DashboardIndexViewModel
    {
        public List<GetBalanceViewModel> Balances { get; set; } = new List<GetBalanceViewModel>();

        public List<GetCardViewModel> Cards { get; set; } = new List<GetCardViewModel>();

        public Dictionary<string, decimal> IncomingWalletTransactions { get; set; }

        public Dictionary<string, decimal> OutgoingWalletTransactions { get; set; }
    }
}

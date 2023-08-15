using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.Dto.ViewModels.ExchangeViewModels;

namespace VirtualWallet.Dto.ViewModels.WalletTransactionViewModels
{
    public class PaginateWalletTransactions : WalletTransactionQueryParameters
    {
        public int? Page { get; set; }
        public List<GetWalletTransactionViewModel> WalletTransactions { get; set; } = new List<GetWalletTransactionViewModel>();
    }
}

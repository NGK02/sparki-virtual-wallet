using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.CustomAttributes;
using VirtualWallet.Dto.TransactionDto;
using VirtualWallet.Dto.ViewModels.CurrencyViewModels;

namespace VirtualWallet.Dto.ViewModels.WalletTransactionViewModels
{
    public class CreateWalletTransactionViewModel
    {
        [HasRecipientIdentifier]
        public string RecipientUsername { get; set; }
        public string RecipientEmail { get; set; }
        public string RecipientPhoneNumber { get; set; }

        public int CurrencyId { get; set; }

        public decimal Amount { get; set; }

        public List<CurrencyViewModel> Currencies { get; set; }
    }
}

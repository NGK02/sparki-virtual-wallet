using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string RecipientIdentifier { get; set; }

        [Required]
        public string RecipientIdentifierValue { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [Required]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Amount must be between {0} and {1}!")]
        public decimal Amount { get; set; }
    }
}

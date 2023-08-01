using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Dto.ViewModels.TransferViewModels
{
    public class TransferViewModel
    {
        [Required]
        public bool HasCardSender { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "The {0} must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required]
        public int CardId { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [Required]
        public int WalletId { get; set; }
    }
}
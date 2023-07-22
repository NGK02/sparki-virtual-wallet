using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.DataAccess.Models
{
    public class Transfer : Entity
    {
        public Card Card { get; set; }

        public Currency CardCurrency { get; set; }

        public Currency WalletCurrency { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "The {0} must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Card currency is required.")]
        public int CardCurrencyId { get; set; }

        public int CardId { get; set; }

        public int Id { get; set; }

        [Required(ErrorMessage = "Wallet currency is required.")]
        public int WalletCurrencyId { get; set; }

        public int WalletId { get; set; }

        public Wallet Wallet { get; set; }
    }
}
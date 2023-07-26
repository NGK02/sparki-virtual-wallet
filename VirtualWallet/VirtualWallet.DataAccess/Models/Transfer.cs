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
        [Required]
        public bool HasCardSender { get; set; }

        [Required]
        public Card Card { get; set; }

        [Required]
        public Currency Currency { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "The {0} must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required]
        public int CardId { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        public int Id { get; set; }

        [Required]
        public int WalletId { get; set; }

        [Required]
        public Wallet Wallet { get; set; }
    }
}
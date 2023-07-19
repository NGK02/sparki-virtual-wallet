using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Contracts;

namespace VirtualWallet.DataAccess.Models
{
    public class Card : ICard
    {
        public DateTime ExpirationDate { get; set; }

        [Range(100, 999, ErrorMessage = "The {0} must be a 3-digit number.")]
        public int CheckNumber { get; set; }

        public int Id { get; set; }

        public int UserId { get; set; } // Foreign key

        [RegularExpression(@"^\d{16}$", ErrorMessage = "The {0} must be a 16-digit number.")]
        public long CardNumber { get; set; }

        [StringLength(30, MinimumLength = 2, ErrorMessage = "The {0} must be between {2} and {1} characters long.")]
        public string CardHolder { get; set; }

        public User User { get; set; } // Navigation property
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Contracts;

namespace VirtualWallet.DataAccess.Models
{
    public class DebitCard : ICard
    {
        public DateTime ExpirationDate { get; set; }

        public int CardNumber { get; set; }

        public int CheckNumber { get; set; }

        public int DebitCardId { get; set; }

        public int UserId { get; set; } // Foreign key

        public string CardHolder { get; set; }

        public User User { get; set; } // Navigation property
    }
}
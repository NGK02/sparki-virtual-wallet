using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.DataAccess.Contracts
{
    public interface ICard
    {
        DateTime ExpirationDate { get; set; }

        int CheckNumber { get; set; }

        long CardNumber { get; set; }

        string CardHolder { get; set; }
    }
}
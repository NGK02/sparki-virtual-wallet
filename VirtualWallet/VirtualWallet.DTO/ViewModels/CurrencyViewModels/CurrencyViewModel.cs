using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Enums;

namespace VirtualWallet.Dto.ViewModels.CurrencyViewModels
{
    public class CurrencyViewModel
    {
        public CurrencyCode Code { get; set; }

        public int Id { get; set; }
    }
}

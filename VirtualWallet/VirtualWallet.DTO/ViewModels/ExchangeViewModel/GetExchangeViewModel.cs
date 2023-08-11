using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Dto.ViewModels.ExchangeViewModel
{
    public class GetExchangeViewModel
    {
        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public decimal Amount { get; set; }

        public decimal ExchangedAmout { get; set; }

        public decimal Rate { get; set; }

        public string Date { get; set; }
    }
}

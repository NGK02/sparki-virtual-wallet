using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Dto.ViewModels.ExchangeViewModels
{
    public class PaginateExchanges : QueryParameters
    {
        public int? Page { get; set; }
        public List<GetExchangeViewModel> Exchanges { get; set; } = new List<GetExchangeViewModel>();
    }
}

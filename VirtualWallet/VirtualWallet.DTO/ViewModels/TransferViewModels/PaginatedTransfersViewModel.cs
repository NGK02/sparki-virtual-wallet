using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.Dto.ViewModels.ExchangeViewModel;

namespace VirtualWallet.Dto.ViewModels.TransferViewModels
{
	public class PaginatedTransfersViewModel : QueryParameters
	{
		public int? Page { get; set; }

		public List<GetTransferViewModel> Transfers { get; set; } = new List<GetTransferViewModel>();
	}
}
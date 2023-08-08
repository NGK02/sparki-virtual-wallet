using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.CreateExcahngeDto;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface IWalletService
    {
        Balance CreateWalletBalance(int currencyId, int walletId);

        IEnumerable<Wallet> GetWallets(int userId);

        // Task<decimal> ExchangeCurrencyAsync(User user, CreateExcahngeDto excahngeValues);

        bool ValidateFunds(Wallet wallet, CreateExcahngeDto excahngeValues);

        Task<Exchange> ExchangeFunds(CreateExcahngeDto excahngeValues, int userId, int walletId);

        Wallet GetWalletById(int walletId);
	}
}
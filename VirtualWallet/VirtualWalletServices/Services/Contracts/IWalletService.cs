using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.ExchangeDto;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface IWalletService
    {
        /// <summary>
        /// Creates a new wallet balance for the specified currency and wallet.
        /// </summary>
        /// <param name="currencyId">The ID of the currency associated with the wallet balance.</param>
        /// <param name="walletId">The ID of the wallet to associate the balance with.</param>
        /// <returns>The newly created wallet balance.</returns>
        Balance CreateWalletBalance(int currencyId, int walletId);

        /// <summary>
        /// Validates whether sufficient funds are available in the wallet for an exchange.
        /// </summary>
        /// <param name="wallet">The wallet to validate funds for.</param>
        /// <param name="exchangeValues">The details of the exchange.</param>
        /// <returns>True if the wallet has sufficient funds for the exchange, otherwise false.</returns>
        bool ValidateFunds(Wallet wallet, CreateExcahngeDto excahngeValues);

        /// <summary>
        /// Retrieves all wallet balances associated with the specified wallet.
        /// </summary>
        /// <param name="walletId">The ID of the wallet to retrieve balances for.</param>
        /// <returns>A list of wallet balances associated with the wallet.</returns>
        List<Balance> GetWalletBalances(int walletId);

        /// <summary>
        /// Initiates an exchange of funds based on the provided exchange details, user ID, and wallet ID.
        /// </summary>
        /// <param name="exchangeValues">The details of the exchange.</param>
        /// <param name="userId">The ID of the user initiating the exchange.</param>
        /// <param name="walletId">The ID of the wallet for the exchange.</param>
        /// <returns>The exchanged funds in the form of an Exchange object.</returns>
        Task<Exchange> ExchangeFunds(CreateExcahngeDto excahngeValues, int userId, int walletId);

        /// <summary>
        /// Distributes funds to the referrer and referred user for successful referrals.
        /// </summary>
        /// <param name="referrerId">The ID of the referrer user.</param>
        /// <param name="referredUserId">The ID of the user referred by the referrer.</param>
        /// <param name="amount">The amount to be distributed.</param>
        /// <param name="currencyId">The ID of the currency for the distribution.</param>
        void DistributeFundsForReferrals(int referrerId, int referredUserId, decimal amount, int currencyId);

        /// <summary>
        /// Retrieves a wallet by its ID.
        /// </summary>
        /// <param name="walletId">The ID of the wallet to retrieve.</param>
        /// <returns>The wallet with the specified ID if found, otherwise null.</returns>
        Wallet GetWalletById(int walletId);
    }
}
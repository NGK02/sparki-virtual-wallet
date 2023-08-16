using System.Transactions;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.Business.Services
{
    public class TransferService : ITransferService
	{
		private readonly IAuthManager authManager;
		private readonly ICurrencyService currencyService;
		private readonly ITransferRepository transferRepository;
		private readonly IUserService userService;
		private readonly IWalletService walletService;

		public TransferService(IAuthManager authManager, ICurrencyService currencyService, ITransferRepository transferRepository, IUserService userService, IWalletService walletService)
		{
			this.authManager = authManager;
			this.currencyService = currencyService;
			this.transferRepository = transferRepository;
			this.userService = userService;
			this.walletService = walletService;
		}

		private void TransferFromWallet(Transfer transfer)
		{
			var wallet = walletService.GetWalletById(transfer.WalletId);

			var balance = wallet.Balances.SingleOrDefault(b => b.CurrencyId == transfer.CurrencyId);
			var currency = currencyService.GetCurrencyById(transfer.CurrencyId);

			if (balance == null)
			{
				throw new InsufficientFundsException($"Cannot withdraw from the wallet. No balance with currency '{currency.Code}'.");
			}

			decimal amountToWithdraw = transfer.Amount;
			decimal availableBalance = balance.Amount;

			if (amountToWithdraw > availableBalance)
			{
				throw new InsufficientFundsException($"Insufficient funds. Available balance: {availableBalance} {balance.Currency.Code}.");
			}

			using (TransactionScope transactionScope = new TransactionScope())
			{
				balance.Amount -= transfer.Amount;
				transferRepository.AddTransfer(transfer);

				transactionScope.Complete();
			}
		}

		private void TransferToWallet(Transfer transfer)
		{
			var wallet = walletService.GetWalletById(transfer.WalletId);

			var balance = wallet.Balances.SingleOrDefault(b => b.CurrencyId == transfer.CurrencyId);

			if (balance == null)
			{
				balance = new Balance { CurrencyId = transfer.CurrencyId, WalletId = transfer.WalletId };
				wallet.Balances.Add(balance);
			}

			using (TransactionScope transactionScope = new TransactionScope())
			{
				balance.Amount += transfer.Amount;
				transferRepository.AddTransfer(transfer);

				transactionScope.Complete();
			}
		}

		public IEnumerable<Transfer> GetTransfers(int userId)
		{
			var user = userService.GetUserById(userId);

			if (!authManager.IsAdmin(user))
			{
				throw new UnauthorizedOperationException("Access to all transfers is restricted.");
			}

			var transfers = transferRepository.GetTransfers();

			if (!transfers.Any() || transfers == null)
			{
				throw new EntityNotFoundException("No transfers available.");
			}

			return transfers;
		}

		public IEnumerable<Transfer> GetUserTransfers(int userId)
		{
			var transfers = transferRepository.GetUserTransfers(userId);

			if (!transfers.Any() || transfers == null)
			{
				throw new EntityNotFoundException("No transfers available.");
			}

			return transfers;
		}

        public IEnumerable<Transfer> GetUserTransfers(int userId, QueryParams parameters)
        {
            var transfers = transferRepository.GetUserTransfers(userId, parameters);

            if (!transfers.Any() || transfers == null)
            {
                throw new EntityNotFoundException("No transfers available.");
            }

            return transfers;
        }

        public Transfer GetTransferById(int transferId, int userId)
		{
			var transfer = transferRepository.GetTransferById(transferId);

			if (transfer == null)
			{
				throw new EntityNotFoundException("Requested transfer not found.");
			}

			var user = userService.GetUserById(userId);

			if (!authManager.IsAdmin(user) && transfer.WalletId != userId)
			{
				throw new UnauthorizedOperationException("Access to transfer denied.");
			}

			return transfer;
		}

		public void CreateTransfer(Transfer transfer)
		{
			if (transfer.HasCardSender)
			{
				TransferToWallet(transfer);
			}
			else
			{
				TransferFromWallet(transfer);
			}
		}

		public void DeleteTransfer(int transferId, int userId)
		{
			var user = userService.GetUserById(userId);

			if (!authManager.IsAdmin(user))
			{
				throw new UnauthorizedOperationException("Insufficient privileges to delete transfer details.");
			}

			var transferToDelete = GetTransferById(transferId, userId);

			transferRepository.DeleteTransfer(transferToDelete);
		}
	}
}
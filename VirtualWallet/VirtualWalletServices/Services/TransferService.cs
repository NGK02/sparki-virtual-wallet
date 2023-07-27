using System.Transactions;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.Business.Services
{
	public class TransferService : ITransferService
	{
		private readonly IAuthManager authManager;
		private readonly ITransferRepository transferRepository;
		private readonly IUserService userService;
		private readonly IWalletService walletService;

		public TransferService(IAuthManager authManager, ITransferRepository transferRepository, IUserService userService, IWalletService walletService)
		{
			this.authManager = authManager;
			this.transferRepository = transferRepository;
			this.userService = userService;
			this.walletService = walletService;
		}

		private void TransferFromWallet(Transfer transfer, int userId)
		{
			var wallet = walletService.GetWalletById(transfer.WalletId, userId);
			var balance = wallet.Balances.SingleOrDefault(b => b.CurrencyId == transfer.CurrencyId);

			if (balance == null)
			{
				throw new InsufficientFundsException($"Cannot withdraw from the wallet. No balance with currency '{transfer.Currency.Code}'.");
			}

			decimal amountToWithdraw = transfer.Amount;
			decimal availableBalance = balance.Amount;

			if (amountToWithdraw > availableBalance)
			{
				throw new InsufficientFundsException($"Insufficient funds. Available balance: {availableBalance} {balance.Currency.Code}.");
			}

			balance.Amount -= transfer.Amount;
			transferRepository.AddTransfer(transfer);
		}

		private void TransferToWallet(Transfer transfer, int userId)
		{
			var wallet = walletService.GetWalletById(transfer.WalletId, userId);
			var balance = wallet.Balances.SingleOrDefault(b => b.CurrencyId == transfer.CurrencyId);

			if (balance == null)
			{
				balance = walletService.CreateWalletBalance(wallet.Id, transfer.CurrencyId);
			}

			using (TransactionScope transactionScope = new TransactionScope())
			{
				balance.Amount += transfer.Amount;
				transferRepository.AddTransfer(transfer);
				transactionScope.Complete();
				//catch (OverflowException)
				//{
				//	throw new InvalidOperationException("Too much money?");
				//}
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
			var user = userService.GetUserById(userId);

			var transfers = transferRepository.GetUserTransfers(user.WalletId);

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

			if (!authManager.IsAdmin(user) && transfer.Wallet.UserId != userId)
			{
				throw new UnauthorizedOperationException("Access to transfer denied.");
			}

			return transfer;
		}

		public void AddTransfer(Transfer transfer, int userId)
		{
			if (transfer.HasCardSender)
			{
				TransferToWallet(transfer, userId);
			}
			else
			{
				TransferFromWallet(transfer, userId);
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
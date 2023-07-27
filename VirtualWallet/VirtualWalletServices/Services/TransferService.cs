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

		private void TransferFromWallet(int userId, Transfer transfer)
		{
            using (TransactionScope transactionScope = new TransactionScope())
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

                transactionScope.Complete();
            }
		}

		private void TransferToWallet(int userId, Transfer transfer)
		{
            using (TransactionScope transactionScope = new TransactionScope())
            {
                var wallet = walletService.GetWalletById(transfer.WalletId, userId);

                var balance = wallet.Balances.SingleOrDefault(b => b.CurrencyId == transfer.CurrencyId);

                if (balance == null)
                {
                    balance = walletService.CreateWalletBalance(wallet.Id, transfer.CurrencyId);
                }

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
			var user = userService.GetUserById(userId);

			var transfers = transferRepository.GetWalletTransfers(user.WalletId);

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

		public void AddTransfer(int userId, Transfer transfer)
		{
			if (transfer.HasCardSender)
			{
				TransferToWallet(userId, transfer);
			}
			else
			{
				TransferFromWallet(userId, transfer);
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
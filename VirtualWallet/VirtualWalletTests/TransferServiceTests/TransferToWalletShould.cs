using Moq;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.TransferServiceTests
{
    [TestClass]
    public class TransferToWalletShould
    {
        private Mock<ICurrencyService> currencyServiceMock;
        private Mock<ITransferRepository> transferRepositoryMock;
        private Mock<IWalletService> walletServiceMock;
        private TransferService transferService;

        [TestInitialize]
        public void Initialize()
        {
            currencyServiceMock = new Mock<ICurrencyService>();
            transferRepositoryMock = new Mock<ITransferRepository>();
            walletServiceMock = new Mock<IWalletService>();

            transferService = new TransferService(null, currencyServiceMock.Object, transferRepositoryMock.Object, null, walletServiceMock.Object);
        }

        [TestMethod]
        public void CreateTransfer_TransferToWallet_SuccessfulTransfer()
        {
            var walletId = 123;
            var currencyId = 1;
            var initialBalanceAmount = 100;

            var wallet = new Wallet { Id = walletId, Balances = new List<Balance>() };
            walletServiceMock.Setup(service => service.GetWalletById(walletId)).Returns(wallet);

            var currency = new Currency { Id = currencyId };
            currencyServiceMock.Setup(service => service.GetCurrencyById(currencyId)).Returns(currency);

            var balance = new Balance { CurrencyId = currencyId, Amount = initialBalanceAmount, Currency = currency };
            wallet.Balances.Add(balance);

            var transfer = new Transfer { WalletId = wallet.Id, CurrencyId = balance.CurrencyId, Amount = 50, HasCardSender = true };
            transferService.CreateTransfer(transfer);

            transferRepositoryMock.Verify(repo => repo.AddTransfer(transfer), Times.Once);

            Assert.AreEqual(initialBalanceAmount + transfer.Amount, balance.Amount);
        }

        [TestMethod]
        public void TransferToWallet_AddsNewBalanceIfNotExists()
        {
            var wallet = new Wallet { Id = 1, Balances = new List<Balance>() };
            var transfer = new Transfer { WalletId = wallet.Id, CurrencyId = 2, Amount = 50, HasCardSender = true };

            walletServiceMock.Setup(service => service.GetWalletById(wallet.Id)).Returns(wallet);

            var existingBalances = new List<Balance>();
            wallet.Balances = existingBalances;

            transferService.CreateTransfer(transfer);

            transferRepositoryMock.Verify(repo => repo.AddTransfer(transfer), Times.Once);

            Assert.AreEqual(1, wallet.Balances.Count);

            var addedBalance = wallet.Balances.FirstOrDefault();
            Assert.IsNotNull(addedBalance);
            Assert.AreEqual(transfer.CurrencyId, addedBalance.CurrencyId);
            Assert.AreEqual(transfer.WalletId, addedBalance.WalletId);
            Assert.AreEqual(transfer.Amount, addedBalance.Amount);
        }
    }
}

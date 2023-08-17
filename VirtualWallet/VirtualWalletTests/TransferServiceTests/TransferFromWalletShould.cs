using Moq;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.TransferServiceTests
{
    [TestClass]
    public class TransferFromWalletShould
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
        public void CreateTransfer_InsufficientBalance_ThrowsInsufficientFundsException()
        {
            decimal transferAmount = 1000;
            var currencyId = 1;
            var walletId = 123;

            var wallet = new Wallet { Id = walletId, Balances = new List<Balance>() };
            walletServiceMock.Setup(service => service.GetWalletById(walletId)).Returns(wallet);

            var currency = new Currency { Id = currencyId };
            currencyServiceMock.Setup(service => service.GetCurrencyById(currencyId)).Returns(currency);

            var balance = new Balance { CurrencyId = currencyId, Amount = 500, Currency = currency };
            wallet.Balances.Add(balance);

            var transfer = new Transfer { WalletId = walletId, CurrencyId = currencyId, Amount = transferAmount };

            Assert.ThrowsException<InsufficientFundsException>(() => transferService.CreateTransfer(transfer));
        }

        [TestMethod]
        public void CreateTransfer_NullBalance_ThrowsInsufficientFundsException()
        {
            var walletId = 123;
            var currencyId = 1;
            decimal transferAmount = 1000;

            var wallet = new Wallet { Id = walletId, Balances = new List<Balance>() };

            wallet.Balances.Add(new Balance { CurrencyId = 2 });

            walletServiceMock.Setup(service => service.GetWalletById(walletId)).Returns(wallet);

            var currency = new Currency { Id = currencyId };
            currencyServiceMock.Setup(service => service.GetCurrencyById(currencyId)).Returns(currency);

            var transfer = new Transfer { WalletId = walletId, CurrencyId = currencyId, Amount = transferAmount };

            Assert.ThrowsException<InsufficientFundsException>(() => transferService.CreateTransfer(transfer));
        }

        [TestMethod]
        public void CreateTransfer_TransferFromWallet_SuccessfulTransfer()
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

            var transfer = new Transfer { WalletId = wallet.Id, CurrencyId = balance.CurrencyId, Amount = 50 };

            transferService.CreateTransfer(transfer);
            transferRepositoryMock.Verify(repo => repo.AddTransfer(transfer), Times.Once);

            Assert.AreEqual(initialBalanceAmount - transfer.Amount, balance.Amount);
        }
    }
}
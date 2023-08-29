using Moq;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.ExchangeDto;

namespace VirtualWalletTests.WalletServiceTests
{
    [TestClass]
    public class ValidateFundsShould
    {
        [TestMethod]
        public void ValidateFundsShould_ValidateSuccessfully()
        {
            var wallet = new Wallet();
            var exchangeValues = new CreateExchangeDto { From = "USD", To = "EUR", Amount = 100.0m };
            var fromCurrency = new Currency { Id = 1 };
            var toCurrency = new Currency { Id = 2 };

            var currencyServiceMock = new Mock<ICurrencyService>();
            currencyServiceMock.Setup(mock => mock.GetCurrencyByCode("USD")).Returns(fromCurrency);
            currencyServiceMock.Setup(mock => mock.GetCurrencyByCode("EUR")).Returns(toCurrency);

            var fromBalance = new Balance { CurrencyId = fromCurrency.Id, Amount = 150.0m };
            wallet.Balances.Add(fromBalance);

            var walletService = new WalletService(currencyServiceMock.Object, null, null, null);

            var result = walletService.ValidateFunds(wallet, exchangeValues);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateFundsShould_ThrowException_WhenSameCurrencies()
        {
            var wallet = new Wallet();

            var currencyServiceMock = new Mock<ICurrencyService>();
            var walletService = new WalletService(currencyServiceMock.Object, null, null, null);

            var exchangeValues = new CreateExchangeDto { From = "USD", To = "USD", Amount = 100.0m };

            Assert.ThrowsException<ArgumentException>(() => walletService.ValidateFunds(wallet, exchangeValues));
        }

        [TestMethod]
        public void ValidateFundsShould_ThrowException_WhenInsufficientFunds()
        {
            var wallet = new Wallet();
            var exchangeValues = new CreateExchangeDto { From = "USD", To = "EUR", Amount = 200.0m };
            var fromCurrency = new Currency { Id = 1 };

            var currencyServiceMock = new Mock<ICurrencyService>();
            currencyServiceMock.Setup(mock => mock.GetCurrencyByCode("USD")).Returns(fromCurrency);

            var fromBalance = new Balance { CurrencyId = fromCurrency.Id, Amount = 150.0m };
            wallet.Balances.Add(fromBalance);

            var walletService = new WalletService(currencyServiceMock.Object, null, null, null);

            Assert.ThrowsException<InsufficientFundsException>(() => walletService.ValidateFunds(wallet, exchangeValues));
        }
    }
}

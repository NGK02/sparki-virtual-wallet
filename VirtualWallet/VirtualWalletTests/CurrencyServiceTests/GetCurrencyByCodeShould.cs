using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.CurrencyServiceTests
{
    [TestClass]
    public class GetCurrencyByCodeShould
    {
        private Mock<ICurrencyRepository> mockCurrencyRepository;
        private CurrencyService currencyService;

        [TestInitialize]
        public void TestInitialize () 
        {
            mockCurrencyRepository = new Mock<ICurrencyRepository>();
            currencyService = new CurrencyService(mockCurrencyRepository.Object);
        }

        [TestMethod]
        public void ReturnCurrency_When_InputIsValid()
        {
            //Arrange
            var currencyCode = CurrencyCode.USD;
            string currencyCodeString = currencyCode.ToString();
            var validCurrency = new Currency { Code = currencyCode, Id = (int)currencyCode };

            mockCurrencyRepository.Setup(cr => cr.GetCurrencyByCode(currencyCode)).Returns(validCurrency);

            //Act
            var result = currencyService.GetCurrencyByCode(currencyCodeString);

            //Assert
            Assert.AreEqual(validCurrency, result);
        }

        [TestMethod]
        public void Throw_When_InputIsInvalid()
        {
            //Arrange
            string currencyCodeString = string.Empty;

            //Act & Assert
            Assert.ThrowsException<ArgumentException>(() => currencyService.GetCurrencyByCode(currencyCodeString));

        }

        [TestMethod]
        public void Throw_When_CurrencyNotFound()
        {
            //Arrange
            var currencyCode = CurrencyCode.USD;
            string currencyCodeString = currencyCode.ToString();

            mockCurrencyRepository.Setup(cr => cr.GetCurrencyByCode(currencyCode)).Returns((Currency)null);

            //Act & Assert
            Assert.ThrowsException<EntityNotFoundException>(() => currencyService.GetCurrencyByCode(currencyCodeString));
        }
    }
}

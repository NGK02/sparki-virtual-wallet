using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.CurrencyServiceTests
{
    [TestClass]
    public class GetCurrencyByIdShould
    {
        private Mock<ICurrencyRepository> mockCurrencyRepository;
        private CurrencyService currencyService;

        [TestInitialize]
        public void TestInitialize()
        {
            mockCurrencyRepository = new Mock<ICurrencyRepository>();
            currencyService = new CurrencyService(mockCurrencyRepository.Object);
        }

        [TestMethod]
        public void ReturnCurrency_When_InputIsValid()
        {
            //Arrange
            var currencyCode = CurrencyCode.USD;
            var currencyId = (int)currencyCode;
            var validCurrency = new Currency { Id = currencyId };

            mockCurrencyRepository.Setup(cr => cr.GetCurrencyById(currencyId)).Returns(validCurrency);

            //Act
            var result = currencyService.GetCurrencyById(currencyId);

            //Assert
            Assert.AreEqual(validCurrency, result);
        }

        [TestMethod]
        public void Throw_When_CurrencyNotFound()
        {
            //Arrange
            var currencyCode = CurrencyCode.USD;
            var currencyId = (int)currencyCode;

            mockCurrencyRepository.Setup(cr => cr.GetCurrencyById(currencyId)).Returns((Currency)null);

            //Act & Assert
            Assert.ThrowsException<EntityNotFoundException>(() => currencyService.GetCurrencyById(currencyId));
        }
    }
}

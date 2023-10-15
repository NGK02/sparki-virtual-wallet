using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.DataAccess.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualWallet.Business.Exceptions;
using Microsoft.Extensions.Options;
using VirtualWallet.Dto.Config;
using Microsoft.Extensions.Configuration;

namespace VirtualWalletTests.ExchangeServiceTests
{
    [TestClass]
    public class CalculateExchangeResultShould
    {
        private CurrencyCode currency = CurrencyCode.USD;

        [TestMethod]
        public async Task CalculateExchangeResult_ShouldReturnResult()
        {
            var exchangeRepoMock = new Mock<IExchangeRepository>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var userServiceMock = new Mock<IUserService>();
            var config = new Mock<IConfiguration>();

            Dictionary<string, decimal> conversionRates = new Dictionary<string, decimal>();
            conversionRates.Add(CurrencyCode.USD.ToString(), 1.3m);

            var sut = new ExchangeService(exchangeRepoMock.Object, memoryCacheMock.Object, userServiceMock.Object, config.Object);

            decimal result = sut.CalculateExchangeResult(conversionRates, currency, 1000);

            Assert.IsTrue(result is decimal);

        }
        [TestMethod]
        public async Task CalculateExchangeResult_Should_Throw_Exception()
        {
            var exchangeRepoMock = new Mock<IExchangeRepository>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var userServiceMock = new Mock<IUserService>();
            var config = new Mock<IConfiguration>();

            Dictionary<string, decimal> conversionRates = new Dictionary<string, decimal>();


            var sut = new ExchangeService(exchangeRepoMock.Object, memoryCacheMock.Object, userServiceMock.Object, config.Object);

            Assert.ThrowsException<EntityNotFoundException>(() => sut.CalculateExchangeResult(conversionRates, currency, 1000));

        }
    }
}

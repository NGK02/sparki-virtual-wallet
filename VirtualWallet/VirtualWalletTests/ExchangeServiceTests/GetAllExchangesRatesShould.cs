using Microsoft.Extensions.Caching.Memory;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.ExchangeServiceTests
{
    [TestClass]
    public class GetAllExchangesRatesShould
    {
        private CurrencyCode currency = CurrencyCode.USD;
        private readonly string apiKey = "33dcab244a4be6a1beae8f4c";

        [TestMethod]
        public async Task GetAllExchangeRates_ReturnsConversionRates()
        {
            var mockHttp = new MockHttpMessageHandler();
            var forCurr = currency;
            var expectedResponse = @"{""conversion_rates"":{""EUR"":0.85,""JPY"":110.5}}";

            mockHttp.When($"https://v6.exchangerate-api.com/v6/{apiKey}/latest/{forCurr}")
                    .Respond("application/json", expectedResponse);

            var exchangeRepoMock = new Mock<IExchangeRepository>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var userServiceMock = new Mock<IUserService>();

            var sut = new ExchangeService(exchangeRepoMock.Object, memoryCacheMock.Object, userServiceMock.Object);


            // Act
            var result = await sut.GetAllExchangeRates(forCurr);

            // Assert

            Assert.AreEqual(162, result.Count);


        }

        [TestMethod]
        public async Task GetExchangeRate_RetrunsRate()
        {
            var exchangeRepoMock = new Mock<IExchangeRepository>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var userServiceMock = new Mock<IUserService>();

            var sut = new ExchangeService(exchangeRepoMock.Object, memoryCacheMock.Object, userServiceMock.Object);
            var result = await sut.GetExchangeRate(CurrencyCode.USD.ToString(), CurrencyCode.EUR.ToString());

            Assert.IsNotNull(result);

        }

        [TestMethod]
        public async Task GetExchangeRateAndExchangedResult_ReturnsResult()
        {
            var exchangeRepoMock = new Mock<IExchangeRepository>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var userServiceMock = new Mock<IUserService>();

            var sut = new ExchangeService(exchangeRepoMock.Object, memoryCacheMock.Object, userServiceMock.Object);
            var result = await sut.GetExchangeRateAndExchangedResult(CurrencyCode.USD, CurrencyCode.EUR, 1000);

            Assert.IsNotNull(result);

        }
        [TestMethod]
        public async Task GetExchangeRateAndExchangedResult_WithString_ReturnsResult()
        {
            var exchangeRepoMock = new Mock<IExchangeRepository>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var userServiceMock = new Mock<IUserService>();

            var sut = new ExchangeService(exchangeRepoMock.Object, memoryCacheMock.Object, userServiceMock.Object);
            var result = await sut.GetExchangeRateAndExchangedResult(CurrencyCode.USD.ToString(), CurrencyCode.EUR.ToString(), 1000.ToString());

            Assert.IsNotNull(result);

        }
        [TestMethod]
        public void CreateExchange_Should_Create()
        {
            Exchange exchange = new Exchange();
            User user = new User { Wallet = new Wallet() };
            var exchangeRepoMock = new Mock<IExchangeRepository>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var userServiceMock = new Mock<IUserService>();

            var sut = new ExchangeService(exchangeRepoMock.Object, memoryCacheMock.Object, userServiceMock.Object);

            userServiceMock.Setup(repo => repo.GetUserById(It.IsAny<int>())).Returns(user);
            exchangeRepoMock.Setup(repo => repo.AddExchange(exchange)).Returns(true);

            Assert.IsTrue(sut.CreateExchange(It.IsAny<int>(), exchange));

        }
    }
}

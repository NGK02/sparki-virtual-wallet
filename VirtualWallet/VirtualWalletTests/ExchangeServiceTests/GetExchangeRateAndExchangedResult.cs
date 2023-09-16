using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Repositories.Contracts;
using Microsoft.Extensions.Options;
using VirtualWallet.Dto.Config;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VirtualWalletTests.ExchangeServiceTests
{
    [TestClass]
    public  class GetExchangeRateAndExchangedResult
    {
        [TestMethod]
        public async Task GetExchangeRateAndExchangedResult_ReturnsResult()
        {
            var exchangeRepoMock = new Mock<IExchangeRepository>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var userServiceMock = new Mock<IUserService>();

            var apiKeys = new ApiKeys { ExchangeRateApikey = "33dcab244a4be6a1beae8f4c" };

            var apiKeysOptions = Options.Create(apiKeys);

            var sut = new ExchangeService(exchangeRepoMock.Object, memoryCacheMock.Object, userServiceMock.Object, apiKeysOptions);
            var result = await sut.GetExchangeRateAndExchangedResult(CurrencyCode.USD, CurrencyCode.EUR, 1000);

            Assert.IsNotNull(result);

        }
        [TestMethod]
        public async Task GetExchangeRateAndExchangedResult_WithString_ReturnsResult()
        {
            var exchangeRepoMock = new Mock<IExchangeRepository>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var userServiceMock = new Mock<IUserService>();

            var apiKeys = new ApiKeys { ExchangeRateApikey = "33dcab244a4be6a1beae8f4c" };

            var apiKeysOptions = Options.Create(apiKeys);

            var sut = new ExchangeService(exchangeRepoMock.Object, memoryCacheMock.Object, userServiceMock.Object, apiKeysOptions);
            var result = await sut.GetExchangeRateAndExchangedResult(CurrencyCode.USD.ToString(), CurrencyCode.EUR.ToString(), 1000.ToString());

            Assert.IsNotNull(result);

        }
    }
}

using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;
using Microsoft.Extensions.Options;
using VirtualWallet.Dto.Config;
using Microsoft.Extensions.Configuration;

namespace VirtualWalletTests.ExchangeServiceTests
{
    [TestClass]
    public class CreateExchangeShould
    {
        [TestMethod]
        public void CreateExchange_Should_Create()
        {
            Exchange exchange = new Exchange();
            User user = new User { Wallet = new Wallet() };
            var exchangeRepoMock = new Mock<IExchangeRepository>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var userServiceMock = new Mock<IUserService>();
            var config = new Mock<IConfiguration>();

            var sut = new ExchangeService(exchangeRepoMock.Object, memoryCacheMock.Object, userServiceMock.Object, config.Object);

            userServiceMock.Setup(repo => repo.GetUserById(It.IsAny<int>())).Returns(user);
            exchangeRepoMock.Setup(repo => repo.AddExchange(exchange)).Returns(true);

            Assert.IsTrue(sut.CreateExchange(It.IsAny<int>(), exchange));

        }
    }
}

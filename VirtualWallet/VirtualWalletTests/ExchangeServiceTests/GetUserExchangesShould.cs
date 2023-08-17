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
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Migrations;
using VirtualWallet.Business.Exceptions;

namespace VirtualWalletTests.ExchangeServiceTests
{
    [TestClass]
    public class GetUserExchangesShould
    {
        [TestMethod]
        public void GetUserExchanges_Should_Get()
        {
            var exchangeRepoMock = new Mock<IExchangeRepository>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var userServiceMock = new Mock<IUserService>();
            var user = new User();
            var queryParams = new QueryParams();
            List<Exchange> exchanges = new List<Exchange> { new Exchange() };
            var sut = new ExchangeService(exchangeRepoMock.Object, memoryCacheMock.Object, userServiceMock.Object);

            userServiceMock.Setup(repo => repo.GetUserById(It.IsAny<int>())).Returns(user);
            exchangeRepoMock.Setup(repo => repo.GetUserExchanges(It.IsAny<int>(), queryParams)).Returns(exchanges);

            var result = sut.GetUserExchanges(It.IsAny<int>(), queryParams);

            CollectionAssert.AreEqual(exchanges,result.ToList());

        }
        [TestMethod]
        public void GetUserExchanges_Should_Throw_Exception()
        {
            var exchangeRepoMock = new Mock<IExchangeRepository>();
            var memoryCacheMock = new Mock<IMemoryCache>();
            var userServiceMock = new Mock<IUserService>();
            var user = new User();
            var queryParams = new QueryParams();
            List<Exchange> exchanges = new List<Exchange>();
            var sut = new ExchangeService(exchangeRepoMock.Object, memoryCacheMock.Object, userServiceMock.Object);

            userServiceMock.Setup(repo => repo.GetUserById(It.IsAny<int>())).Returns(user);
            exchangeRepoMock.Setup(repo => repo.GetUserExchanges(It.IsAny<int>(), queryParams)).Returns(exchanges);


            Assert.ThrowsException<EntityNotFoundException>(() => sut.GetUserExchanges(It.IsAny<int>(), queryParams));

        }
    }
}

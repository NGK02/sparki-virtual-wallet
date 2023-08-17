using Moq;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.ReferralServiceTests
{
    [TestClass]
    public class CreateReferralShould
    {
        [TestMethod]
        public void CreateReferral_Should_AddReferralForUser()
        {
            int userId = 1;
            var user = new User { Id = userId };
            var referral = new Referral();

            var userServiceMock = new Mock<IUserService>();
            var referralRepositoryMock = new Mock<IReferralRepository>();

            userServiceMock.Setup(service => service.GetUserById(userId))
                           .Returns(user);

            var referralService = new ReferralService(referralRepositoryMock.Object, userServiceMock.Object);

            referralService.CreateReferral(userId, referral);

            Assert.AreEqual(user, referral.Referrer);
            Assert.AreEqual(userId, referral.ReferrerId);
            Assert.IsTrue(user.Referrals.Contains(referral));

            referralRepositoryMock.Verify(repo => repo.CreateReferral(referral), Times.Once);
        }
    }
}

using Moq;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.ReferralServiceTests
{
    [TestClass]
    public class GetReferralByIdShould
    {
        [TestMethod]
        public void GetReferralById_Should_ReturnReferral_WhenReferralExists()
        {
            int referralId = 1;
            var referralRepositoryMock = new Mock<IReferralRepository>();
            var expectedReferral = new Referral { Id = referralId };

            referralRepositoryMock.Setup(repo => repo.FindReferralById(referralId))
                                  .Returns(expectedReferral);

            var referralService = new ReferralService(referralRepositoryMock.Object, null);

            var result = referralService.GetReferralById(referralId);

            Assert.AreEqual(expectedReferral, result);
        }

        [TestMethod]
        public void GetReferralById_Should_ThrowEntityNotFoundException_WhenReferralDoesNotExist()
        {
            int referralId = 1;
            var referralRepositoryMock = new Mock<IReferralRepository>();

            referralRepositoryMock.Setup(repo => repo.FindReferralById(referralId))
                                  .Returns((Referral)null);

            var referralService = new ReferralService(referralRepositoryMock.Object, null);

            Assert.ThrowsException<EntityNotFoundException>(() => referralService.GetReferralById(referralId));
        }
    }
}

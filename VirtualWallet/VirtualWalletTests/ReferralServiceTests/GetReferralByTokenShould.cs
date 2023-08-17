using Moq;
using VirtualWallet.Business.Services;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWalletTests.ReferralServiceTests
{
    [TestClass]
    public class GetReferralByTokenShould
    {
        [TestMethod]
        public void GetReferralByToken_Should_ReturnReferral_WhenReferralExists()
        {
            string referralToken = "i_am_a_silly_token_for_a_unit_test";
            var referralRepositoryMock = new Mock<IReferralRepository>();
            var expectedReferral = new Referral { ConfirmationToken = referralToken };

            referralRepositoryMock.Setup(repo => repo.FindReferralByToken(referralToken))
                                  .Returns(expectedReferral);

            var referralService = new ReferralService(referralRepositoryMock.Object, null);

            var result = referralService.GetReferralByToken(referralToken);
            
            Assert.AreEqual(expectedReferral, result);
        }

        [TestMethod]
        public void GetReferralByToken_Should_ReturnNull_WhenReferralDoesNotExist()
        {
            string referralToken = "non_existent_token";
            var referralRepositoryMock = new Mock<IReferralRepository>();

            referralRepositoryMock.Setup(repo => repo.FindReferralByToken(referralToken))
                                  .Returns((Referral) null);

            var referralService = new ReferralService(referralRepositoryMock.Object, null);

            var result = referralService.GetReferralByToken(referralToken);

            Assert.IsNull(result);
        }
    }
}

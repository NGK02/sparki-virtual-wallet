using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface IReferralService
	{
        /// <summary>
        /// Retrieves a referral by its ID.
        /// </summary>
        /// <param name="referralId">The ID of the referral to retrieve.</param>
        /// <returns>The referral with the specified ID if found, otherwise null.</returns>
        Referral GetReferralById(int referralId);

        /// <summary>
        /// Retrieves a referral by its unique token.
        /// </summary>
        /// <param name="token">The unique token associated with the referral.</param>
        /// <returns>The referral with the specified token if found, otherwise null.</returns>
        Referral GetReferralByToken(string token);

        /// <summary>
        /// Creates a new referral for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user for whom the referral is being created.</param>
        /// <param name="referral">The referral object containing the details of the new referral.</param>
        void CreateReferral(int userId, Referral referral);
	}
}
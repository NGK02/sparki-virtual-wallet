using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.Business.Services
{
    public class ReferralService : IReferralService
	{
		private readonly IReferralRepository referralRepository;
		private readonly IUserService userService;

		public ReferralService(IReferralRepository referralRepository, IUserService userService)
		{
			this.referralRepository = referralRepository;
			this.userService = userService;
		}

		public Referral GetReferralById(int referralId)
		{
			var referral = referralRepository.FindReferralById(referralId);

			if (referral == null)
			{
				throw new EntityNotFoundException("Requested referral not found.");
			}

			return referral;
		}

        public Referral GetReferralByToken(string token)
        {
            return referralRepository.FindReferralByToken(token);
        }

        public void CreateReferral(int userId, Referral referral)
		{
			var user = userService.GetUserById(userId);

			referral.Referrer = user;
			referral.ReferrerId = userId;
			user.Referrals.Add(referral);

			referralRepository.CreateReferral(referral);
		}
	}
}
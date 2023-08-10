using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories;
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

		public Referral FindReferralById(int referralId)
		{
			var referral = referralRepository.FindReferralById(referralId);

			if (referral == null)
			{
				throw new EntityNotFoundException("Requested referral not found.");
			}

			return referral;
		}

		public void CreateReferral(int userId, Referral referral)
		{
			var user = userService.GetUserById(userId);

			if (referralRepository.TokenExists(referral.ConfirmationToken))
			{
				throw new ArgumentException("A token with the provided code already exists.");
			}

			referral.Referrer = user;
			referral.ReferrerId = userId;
			user.Referrals.Add(referral);

			referralRepository.CreateReferral(referral);
		}

		public Referral FindReferralByToken(string token)
		{
			var referral = referralRepository.FindReferralByToken(token);

			if (referral == null)
			{
				throw new EntityNotFoundException("Requested referral not found.");
			}

			return referral;
		}
	}
}
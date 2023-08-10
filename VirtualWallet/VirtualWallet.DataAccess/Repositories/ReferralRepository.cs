using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Repositories.Contracts;

namespace VirtualWallet.DataAccess.Repositories
{
	public class ReferralRepository : IReferralRepository
	{
		private readonly WalletDbContext walletDbContext;

		public ReferralRepository(WalletDbContext walletDbContext)
		{
			this.walletDbContext = walletDbContext;
		}

		private IQueryable<Referral> GetQueryableReferrals()
		{
			return walletDbContext.Referrals.Where(r => !r.IsDeleted).Include(r => r.Referrer);
		}

		public bool TokenExists(string token)
		{
			return GetQueryableReferrals().Any(r => r.ConfirmationToken == token);
		}

		public Referral FindReferralById(int referralId)
		{
			return GetQueryableReferrals().SingleOrDefault(r => r.Id == referralId);
		}

		public void CreateReferral(Referral referral)
		{
			referral.CreatedOn = DateTime.Now;
			walletDbContext.Referrals.Add(referral);
			walletDbContext.SaveChanges();
		}

		public Referral FindReferralByToken(string token)
		{
			return GetQueryableReferrals().SingleOrDefault(r => r.ConfirmationToken == token);
		}
	}
}
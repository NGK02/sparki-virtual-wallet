using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.DataAccess.Repositories.Contracts
{
	public interface IReferralRepository
	{
		bool TokenExists(string token);

		Referral FindReferralById(int referralId);

		Referral FindReferralByToken(string token);

		void CreateReferral(Referral referral);
	}
}
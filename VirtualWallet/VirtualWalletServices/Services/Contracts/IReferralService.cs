using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Business.Services.Contracts
{
	public interface IReferralService
	{
		Referral FindReferralById(int referralId);

		Referral FindReferralByToken(string token);

		void CreateReferral(int userId, Referral referral);
	}
}
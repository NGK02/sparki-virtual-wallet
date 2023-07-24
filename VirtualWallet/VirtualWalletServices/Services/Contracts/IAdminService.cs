using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services.Contracts
{
	public interface IAdminService
	{
		List<User> GetUsers(UserQueryParameters queryParameters);

		bool BlockUser(int? id, string username);

		bool UnBlockUser(int? id, string username);

	}
}

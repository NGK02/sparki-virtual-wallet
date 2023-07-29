﻿namespace VirtualWallet.Web.Helper.Contracts
{
	public interface IAuthManagerMVC
	{
		/// <summary>
		/// Check if there is logged user.
		/// <param key="LoggedUser">Key paraim is LoggedUser from session.</param>
		/// </summary>
		bool isLogged(string key);

		/// <summary>
		/// Check if session logged user is admin.
		/// <param key="roleId">Key paraim is roleId from session.</param>
		/// </summary>
		bool isAdmin(string key);

		/// <summary>
		/// Check if logged user ID matches the content creator ID.
		/// <param key="userId">Key paraim is userId from session.</param>
		/// </summary>
		public bool isContentCreator(string key, int contentCreatorId);

		/// <summary>
		/// Check if logged user is blocked.
		/// <param key="roleId">Key paraim is roleId from session.</param>
		/// </summary>
		public bool isBlocked(string key);
	}
}

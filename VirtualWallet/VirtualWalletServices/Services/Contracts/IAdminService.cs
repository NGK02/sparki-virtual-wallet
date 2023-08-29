using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface IAdminService
	{
        /// <summary>
        /// Blocks a user based on the provided identification details.
        /// </summary>
        /// <param name="id">The ID of the user to block, if available.</param>
        /// <param name="username">The username of the user to block, if available.</param>
        /// <param name="email">The email of the user to block, if available.</param>
        /// <param name="phoneNumber">The phone number of the user to block, if available.</param>
        /// <returns>True if the user was successfully blocked, otherwise false.</returns>
        bool BlockUser(int? id, string username, string email, string phoneNumber);

        /// <summary>
        /// Unblocks a previously blocked user based on the provided identification details.
        /// </summary>
        /// <param name="id">The ID of the user to unblock, if available.</param>
        /// <param name="username">The username of the user to unblock, if available.</param>
        /// <param name="email">The email of the user to unblock, if available.</param>
        /// <param name="phoneNumber">The phone number of the user to unblock, if available.</param>
        /// <returns>True if the user was successfully unblocked, otherwise false.</returns>
        bool UnblockUser(int? id, string username, string email, string phoneNumber);

        /// <summary>
        /// Retrieves a list of users based on the provided query parameters.
        /// </summary>
        /// <param name="queryParameters">Parameters to filter or modify the user list.</param>
        /// <returns>A list of users based on the specified criteria.</returns>
        List<User> GetUsers(UserQueryParameters queryParameters);
    }
}
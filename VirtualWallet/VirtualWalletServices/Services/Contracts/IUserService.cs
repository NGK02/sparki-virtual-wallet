using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface IUserService
	{
        /// <summary>
        /// Creates a new user using the provided user object.
        /// </summary>
        /// <param name="mappedUser">The user object containing user information.</param>
        /// <returns>True if the user was successfully created, otherwise false.</returns>
        bool CreateUser(User mappedUser);

        /// <summary>
        /// Deletes a user by their username or ID.
        /// </summary>
        /// <param name="userName">The username of the user to delete.</param>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>True if the user was successfully deleted, otherwise false.</returns>
        bool DeleteUser(string userName, int? userId);

        /// <summary>
        /// Checks if an email exists in the system.
        /// </summary>
        /// <param name="email">The email to check for existence.</param>
        /// <returns>True if the email exists, otherwise false.</returns>
        bool EmailExists(string email);

        /// <summary>
        /// Retrieves the count of users in the system.
        /// </summary>
        /// <returns>The count of users in the system.</returns>
        int GetUsersCount();

        /// <summary>
        /// Retrieves a list of all users in the system.
        /// </summary>
        /// <returns>A list of all users in the system.</returns>
        List<User> GetUsers();

        /// <summary>
        /// Retrieves a user by their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>The user with the specified email if found, otherwise null.</returns>
        User GetUserByEmail(string email);

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The user with the specified ID if found, otherwise null.</returns>
        User GetUserById(int userId);

        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="userName">The username of the user to retrieve.</param>
        /// <returns>The user with the specified username if found, otherwise null.</returns>
        User GetUserByUsername(string userName);

        /// <summary>
        /// Searches for a user based on the provided query parameters.
        /// </summary>
        /// <param name="queryParams">Parameters to filter or modify the user search.</param>
        /// <returns>The user matching the specified criteria if found, otherwise null.</returns>
        User SearchBy(UserQueryParameters queryParams);

        /// <summary>
        /// Updates an existing user's information with the provided new values.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="userNewValues">The user object containing the updated information.</param>
        /// <returns>The updated user object if the update was successful, otherwise null.</returns>
        User UpdateUser(int id, User userNewValues);

        /// <summary>
        /// Updates an existing user's information with the provided new values.
        /// </summary>
        /// <param name="username">The username of the user to update.</param>
        /// <param name="userNewValues">The user object containing the updated information.</param>
        /// <returns>The updated user object if the update was successful, otherwise null.</returns>
        User UpdateUser(string username, User userNewValues);

        /// <summary>
        /// Confirms a user's status using the provided user object and user ID.
        /// </summary>
        /// <param name="user">The user object to confirm.</param>
        /// <param name="userId">The ID of the user confirming the status.</param>
        void ConfirmUser(User user, int userId);

        /// <summary>
        /// Updates a user's confirmation token using the provided user object and username.
        /// </summary>
        /// <param name="user">The user object to update the confirmation token for.</param>
        /// <param name="username">The username of the user.</param>
        void UpdateUserConfirmationToken(User user, string username);
    }
}
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Business.Services.Contracts
{
    public interface ITransferService
    {
        /// <summary>
        /// Retrieves all transfers while checking authorization for the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user requesting the transfers.</param>
        /// <returns>
        /// An enumerable collection of transfers if the user is authorized to access them,
        /// otherwise an empty collection.
        /// </returns>
        IEnumerable<Transfer> GetTransfers(int userId);

        /// <summary>
        /// Retrieves transfers associated with the specified user.
        /// </summary>
        /// <param name="userId">The ID of the user whose transfers are being retrieved.</param>
        /// <returns>An enumerable collection of transfers associated with the user.</returns>
        IEnumerable<Transfer> GetUserTransfers(int userId);

        /// <summary>
        /// Retrieves transfers associated with the specified user using optional query parameters.
        /// </summary>
        /// <param name="userId">The ID of the user whose transfers are being retrieved.</param>
        /// <param name="parameters">Optional query parameters to filter or modify the results.</param>
        /// <returns>An enumerable collection of transfers based on the specified criteria.</returns>
        IEnumerable<Transfer> GetUserTransfers(int userId, QueryParams parameters);

        /// <summary>
        /// Retrieves a transfer by its ID for the specified user.
        /// </summary>
        /// <param name="transferId">The ID of the transfer to retrieve.</param>
        /// <param name="userId">The ID of the user requesting the transfer.</param>
        /// <returns>The transfer with the specified ID if found, otherwise null.</returns>
        Transfer GetTransferById(int transferId, int userId);

        /// <summary>
        /// Creates a new transfer.
        /// </summary>
        /// <param name="transfer">The transfer object containing the details of the transfer.</param>
        void CreateTransfer(Transfer transfer);

        /// <summary>
        /// Deletes a transfer by its ID for the specified user.
        /// </summary>
        /// <param name="transferId">The ID of the transfer to delete.</param>
        /// <param name="userId">The ID of the user who owns the transfer.</param>
        void DeleteTransfer(int transferId, int userId);
    }
}
using FribergCarRentalsBravo.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentalsBravo.DataAccess.Repositories
{
    /// <summary>
    /// An interface for an admin repository.
    /// </summary>
    public interface IAdminRepository
    {
        #region Methods

        /// <summary>
        /// Updates the admin in the database.
        /// </summary>
        /// <param name="admin">The admin to update.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public Task<Admin> EditAsync(Admin admin);

        /// <summary>
        /// Gets the admin by ID.
        /// </summary>
        /// <remarks>Returned admin will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the admin.</param>
        /// <returns>A <see cref="Task"/> object containg the admin.</returns>
        public Task<Admin> GetAdminByIdAsync(int id);

        /// <summary>
        /// Attempts to fetch an admin with matching email and password.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="email">The email for the admin.</param>
        /// <param name="password">The password for the admin.</param>
        /// <returns>A <see cref="Task"/> object containing the admin if found or null if not found.</returns>
        public Task<Admin?> GetMatchingAdminAsync(string email, string password);

        #endregion
    }
}
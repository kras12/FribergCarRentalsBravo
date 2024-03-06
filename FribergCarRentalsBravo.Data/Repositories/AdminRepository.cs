using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.DataAccess.DatabaseContexts;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Channels;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using FribergCarRentalsBravo.DataAccess.Helpers;

namespace FribergCarRentalsBravo.DataAccess.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        #region Fields

        private readonly ApplicationDbContext applicationDbContext;

        #endregion

        #region Constructors
        public AdminRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        #endregion

        #region Methods

        public async Task<AdminUser> EditAsync(AdminUser admin)
        {
            applicationDbContext.Update(admin);
            await applicationDbContext.SaveChangesAsync();
            return PasswordHelper.RemovePassword(admin)!;
        }

        public async Task<AdminUser?> GetAdminByIdAsync(int id)
        {
            return PasswordHelper.RemovePassword(await applicationDbContext.Admin.FirstOrDefaultAsync(a => a.AdminId == id));
        }

        /// <summary>
        /// Attempts to fetch an admin with matching email and password.
        /// </summary>
        /// <remarks>Returned entities will not be tracked by EF Core.</remarks>
        /// <param name="email">The email for the admin.</param>
        /// <param name="password">The password for the admin.</param>
        /// <returns>A <see cref="Task"/> object containing the admin if found or null if not found.</returns>
        public async Task<AdminUser?> GetMatchingAdminAsync(string email, string password)
        {
            return PasswordHelper.RemovePassword(await applicationDbContext.Admin.AsNoTracking().SingleOrDefaultAsync(x => x.Email == email && x.Password == password));
        }

        #endregion
    }
}

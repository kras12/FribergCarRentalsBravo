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
        public async Task<Admin> EditAsync(Admin admin)
        {
            applicationDbContext.Update(admin);
            await applicationDbContext.SaveChangesAsync();
            return admin;
        }

        public async Task<Admin?> GetAdminByIdAsync(int id)
        {
            return await applicationDbContext.Admin.FirstOrDefaultAsync(a => a.AdminId == id);
        }

        #endregion
    }
}

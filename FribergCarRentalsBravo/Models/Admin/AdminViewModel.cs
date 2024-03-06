using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.Models.Users;

namespace FribergCarRentalsBravo.Models.Admin
{
    /// <summary>
    /// A view model class that handles data for a customer.
    /// </summary>
    public class AdminViewModel : UserViewModelBase
    {
        #region Constructors

        public AdminViewModel()
        {
            
        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="admin">The admin to copy data from.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public AdminViewModel(AdminUser admin) 
            : base(firstName: "", lastName: "", admin.Email)
        {
            AdminId = admin.AdminId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the admin.
        /// </summary>
        [DisplayName("Admin ID")]
        [BindNever]
        public int AdminId { get; set; }

        #endregion
    }
}

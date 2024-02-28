using FribergCarRentalsBravo.Models.Users;
using System.ComponentModel;

namespace FribergCarRentalsBravo.Models.Customers
{
    /// <summary>
    /// A view model that handles data related to user login.
    /// </summary>
    public class LoginCustomerViewModel : UserLoginViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public LoginCustomerViewModel() : base()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="email">The email for the customer.</param>
        /// <param name="password">The password for the customer.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public LoginCustomerViewModel(string email, string password) : base(email, password)
        {

        }

        #endregion
    }
}

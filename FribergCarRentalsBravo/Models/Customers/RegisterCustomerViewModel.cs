using FribergCarRentalsBravo.Models.Users;
using System.ComponentModel;

namespace FribergCarRentalsBravo.Models.Customers
{
    /// <summary>
    /// A view model class that handles data relating to customer registration.
    /// </summary>
    public class RegisterCustomerViewModel : UserCreateViewModel
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public RegisterCustomerViewModel() : base()
        {
            
        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="firstName">The first name for the customer.</param>
        /// <param name="lastName">The last name for the customer.</param>
        /// <param name="email">The email address for the customer.</param>
        /// <param name="password">The password for the customer.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public RegisterCustomerViewModel(string firstName, string lastName, string email, string password) : base(firstName, lastName, email, password)
        {

        }

        #endregion
    }
}

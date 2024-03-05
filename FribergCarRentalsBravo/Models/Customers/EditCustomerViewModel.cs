using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.Models.Users;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalsBravo.Models.Customers
{
    /// <summary>
    /// A view model class that handles data relating to customer data editing.
    /// </summary>
    public class EditCustomerViewModel : UserViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public EditCustomerViewModel() : base()
        {
            
        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="customer">The customer to model.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public EditCustomerViewModel(Customer customer) :
            base(customer.FirstName, customer.LastName, customer.Email)
        {
            #region Checks

            if (customer.CustomerId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(customer.CustomerId), $"The value of parameter '{customer.CustomerId}' can't be negative.");
            }

            #endregion

            CustomerId = customer.CustomerId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the user.
        /// </summary>
        [DisplayName("ID")]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be a positive number larger than 1.")]
        public int CustomerId { get; set; }

        /// <summary>
        /// The password for the user.
        /// </summary>
        [DisplayName("New Password")]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: MaxPasswordLength, MinimumLength = MinPasswordLength, ErrorMessage = PasswordLengthValidationMessage)]
        public string? Password { get; set; }        

        #endregion
    }
}

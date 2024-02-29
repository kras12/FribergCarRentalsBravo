using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentals.Models.Other;

namespace FribergCarRentalsBravo.Models.Customers
{
    /// <summary>
    /// A view model class that handles data for a customer.
    /// </summary>
    public class CustomerViewModel : ViewModelBase
    {
        #region Constructors

        public CustomerViewModel()
        {
            
        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="customer">The customer to copy data from.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public CustomerViewModel(Customer customer)
        {
            CustomerId = customer.CustomerId;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Email = customer.Email;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the customer.
        /// </summary>
        [DisplayName("Customer ID")]
        [BindNever]
        public int CustomerId { get; set; }

        /// <summary>
        /// The email address for the user.
        /// </summary>
        [DisplayName("Email")]
        [Required(AllowEmptyStrings = false)]
        [StringLength(maximumLength: DefaultMaxCharacterInput, ErrorMessage = InputTooLongValidationMessage)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public virtual string Email { get; set; } = "";

        /// <summary>
        /// The first name for the user.
        /// </summary>
        [DisplayName("First Name")]
        [Required]
        [StringLength(maximumLength: DefaultMaxCharacterInput, ErrorMessage = InputTooLongValidationMessage)]
        public virtual string FirstName { get; set; } = "";

        /// <summary>
        /// The full name for the user.
        /// </summary>
        [BindNever]
        [DisplayName("Full Name")]
        public virtual string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        /// <summary>
        /// The last name for the user.
        /// </summary>
        [DisplayName("Last Name")]
        [Required]
        [StringLength(maximumLength: DefaultMaxCharacterInput, ErrorMessage = InputTooLongValidationMessage)]
        public virtual string LastName { get; set; } = "";

        #endregion
    }
}

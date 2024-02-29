using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalsBravo.Models.Users
{
    /// <summary>
    /// A viewmodel base class that handles data related to users. 
    /// </summary>
    /// <remarks>This class acts like a base class for view models of all types as it supports model binding on its properties.</remarks>
    public abstract class UserViewModelBase : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        protected UserViewModelBase()
        {
            
        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="firstName">The first name for the user.</param>
        /// <param name="lastName">The last name for the user.</param>
        /// <param name="email">The email address for the user.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected UserViewModelBase(string firstName, string lastName, string email)
        {
            #region Checks

            if (firstName is null)
            {
                throw new ArgumentNullException(nameof(firstName), $"The value of parameter '{firstName}' can't be null");
            }

            if (lastName is null)
            {
                throw new ArgumentNullException(nameof(lastName), $"The value of parameter '{lastName}' can't be null");
            }

            if (email is null)
            {
                throw new ArgumentNullException(nameof(email), $"The value of parameter '{email}' can't be null");
            }

            #endregion

            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        #endregion

        #region Properties

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

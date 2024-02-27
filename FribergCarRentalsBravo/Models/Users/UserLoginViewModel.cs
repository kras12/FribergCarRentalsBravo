using FribergCarRentals.Models.Other;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalsBravo.Models.Users
{
    /// <summary>
    /// A viewmodel base class that handles data related to user login. 
    /// </summary>
    public abstract class UserLoginViewModel : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor. 
        /// </summary>
        protected UserLoginViewModel()
        {

        }

        /// <summary>
        /// A constructor. 
        /// </summary>
        /// <param name="email">The email for the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected UserLoginViewModel(string email, string password)
        {
            #region Checks

            if (email is null)
            {
                throw new ArgumentNullException(nameof(email), $"The value of parameter '{email}' can't be null.");
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password), $"The value of parameter '{password}' can't be null.");
            }

            #endregion

            Email = email;
            Password = password;
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
        public string Email { get; set; } = "";

        /// <summary>
        /// The password for the user.
        /// </summary>
        [DisplayName("Password")]
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        #endregion
    }
}

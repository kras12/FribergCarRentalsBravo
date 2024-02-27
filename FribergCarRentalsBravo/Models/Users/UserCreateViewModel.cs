using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalsBravo.Models.Users
{
    /// <summary>
    /// A view model class that handles data realting to the creation of users. 
    /// </summary>
    public abstract class UserCreateViewModel : UserViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        protected UserCreateViewModel() : base()
        {
            
        }

        /// <summary>
        ///  A constructor.
        /// </summary>
        /// <param name="firstName">The first name for the user.</param>
        /// <param name="lastName">The last name for the user.</param>
        /// <param name="email">The email address for the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <exception cref="ArgumentNullException"></exception>
        protected UserCreateViewModel(string firstName, string lastName, string email , string password) : base(firstName, lastName, email)
        {
            #region MyRegion

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password), $"The value of parameter '{password}' can't be null");
            }

            #endregion

            Password = password;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The password for the user.
        /// </summary>
        [DisplayName("Password")]
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(maximumLength: MaxPasswordLength, MinimumLength = MinPasswordLength, ErrorMessage = PasswordLengthValidationMessage)]
        public string Password { get; set; } = "";

        #endregion
    }
}

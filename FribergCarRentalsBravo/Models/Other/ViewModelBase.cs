using FribergCarRentalsBravo.Models.Other;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.Models.Other
{
    /// <summary>
    /// A base class for view models.
    /// </summary>
    public abstract class ViewModelBase
    {
        #region RegexPatternConstants

        /// <summary>
        /// Regular expression pattern for email addresses. 
        /// </summary>
        /// <remarks>
        /// <para>Letters, numeric characters, dots, underscores and dashes are allowed before the @ character.</para>
        /// <para>Letters, numeric characters, dots, and dashes are allowed after the @ character and before the last dot.</para>
        /// <para>Only letters are allowed after the last dot.</para>
        /// </remarks>
        protected const string EmailRegexPattern = @"^[\p{L}\p{N}\._\-]+\@[\p{L}\p{N}\.\-]+\.\p{L}+$";

        /// <summary>
        /// Regular expression pattern to only allow letters, numbers and spaces. 
        /// </summary>
        protected const string LettersAndSpacesRegexPattern = @"^[\p{L} ]+$";

        /// <summary>
        /// Regular expression pattern to only allow letters, numbers and spaces. 
        /// </summary>
        protected const string LettersNumbersAndSpacesRegexPattern = @"^[\p{L}\p{N} ]+$";

        /// <summary>
        /// Regular expression pattern for registration numbers (formats abc123 and acb12d). 
        /// </summary>
        protected const string RegistrationNumberRegexPattern = @"^[a-zA-Z]{3}[0-9]{2}[a-zA-Z0-9]{1}$";

        #endregion

        #region ValidationMessagesConstants

        protected const string EmailInputValidationMessage = "The text you entered is not a valid email.";

        /// <summary>
        /// The message to show when the input text is too long. 
        /// </summary>
        protected const string InputTooLongValidationMessage = "The text you entered is longer than 50 characters.";

        /// <summary>
        /// The message to show when an input field has not been filled out. 
        /// </summary>
        protected const string MandatoryFieldValidationMessage = "This field is mandatory.";

        /// <summary>
        /// A message to inform the user that only letters and spaces are allowed as input. 
        /// </summary>
        protected const string OnlyLettersAndSpacesValidationMessage = "Only letters and spaces are allowed as input.";

        /// <summary>
        /// A message to inform the user that only letters, numbers and spaces are allowed as input. 
        /// </summary>
        protected const string OnlyLettersNumbersAndSpacesValidationMessage = "Only letters, numbers and spaces are allowed as input.";

        /// <summary>
        /// The validation message to show when the length of the password doesn't fit the character count interval. 
        /// </summary>
        protected const string PasswordLengthValidationMessage = "The password must be between 6 and 50 characters long.";

        /// <summary>
        /// A message to inform the user about the valid input format for a registration number.
        /// </summary>
        protected const string RegistrationNumberValidationMessage = "Registration numbers must be entered in the format abc123 or acb12d.";

        #endregion

        #region OtherConstants

        /// <summary>
        /// A format string for showing date only (format yyyy-MM-dd).
        /// </summary>
        protected const string DateFormatString = "{0:yyyy-MM-dd}";

        /// <summary>
        /// The default format string for displaying dates. 
        /// </summary>
        protected const string DefaultDateFormatString = "{0:g}";

        /// <summary>
        /// The default float number format string with 2 decimals.
        /// </summary>
        protected const string DefaultFloatNumberInputFormatString = "{0:0.00}";

        /// <summary>
        /// The default integer number format string with thousands separator.
        /// </summary>
        protected const string DefaultIntegerNumberOutputFormatString = "{0:N0}";

        /// <summary>
        /// The default value for max number of characters allowed as input.
        /// </summary>
        protected const int DefaultMaxCharacterInput = 50;
        
        /// <summary>
        /// The default format string for displaying price with 2 decimals, thousands separator, and currency.
        /// </summary>
        protected const string DefaultPriceOutputFormatString = "{0:N2} kr"; 

        /// <summary>
        /// The maximum allowed password length.
        /// </summary>
        protected const int MaxPasswordLength = 50;

        /// <summary>
        /// The minimum allowed password length.
        /// </summary>
        protected const int MinPasswordLength = 6;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="pageTitle">An optional title for the page.</param>
        /// <param name="pageSubTitle">An optional page sub title for the page.</param>
        /// <exception cref="ArgumentException"></exception>
        protected ViewModelBase(string? pageTitle = null, string? pageSubTitle = null)
        {
            PageTitle = pageTitle;
            PageSubTitle = pageSubTitle;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if there are messages that can be shown to the user.
        /// </summary>
        public bool HaveMessages
        {
            get
            {
                return Messages.Count > 0;
            }
        }

        /// <summary>
        /// Returns true if there is a page sub title. 
        /// </summary>
        public bool HavePageSubTitle
        {
            get
            {
                return !string.IsNullOrEmpty(PageSubTitle);
            }
        }

        /// <summary>
        /// Returns true if there is a page title. 
        /// </summary>
        public bool HavePageTitle
        {
            get
            {
                return !string.IsNullOrEmpty(PageTitle);
            }
        }

        /// <summary>
        /// Messages that can be shown to the user.
        /// </summary>
        public List<MessageViewModel> Messages { get; } = new();

        /// <summary>
        /// An optional page sub title for the page. 
        /// </summary>
        [BindNever]
        public string? PageSubTitle { get; set; } = null;

        /// <summary>
        /// The title for the page. 
        /// </summary>
        [BindNever]
        public string? PageTitle { get; set; } = null;

        #endregion

        #region Methods

        /// <summary>
        /// Creates a message that can be shown to the user. 
        /// </summary>
        // <param name="type">The type of the message.</param>
        /// <param name="body">The body of the message.</param>
        /// <param name="title">The optional title of the message. </param>
        /// <exception cref="ArgumentException"></exception>
        public void CreateMessage(MessageType type, string body, string? title = null)
        {
            #region Checks

            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentException($"The value of parameter '{nameof(body)}' can't be empty.", nameof(body));
            }

            #endregion

            Messages.Add(new MessageViewModel(type, body, title));
        }

        #endregion
    }
}

namespace FribergCarRentalsBravo.Sessions
{
    /// <summary>
    /// User roles for logged in users. 
    /// </summary>
    public enum UserRole
    {
        Unknown,
        Customer,
        Admin
    }

    /// <summary>
    /// A class that represents stored user session data. 
    /// </summary>
    public class UserSessionData
    {
        #region Constructors

        /// <summary>
        /// A constructors
        /// </summary>
        /// <param name="userId">The ID for the user.</param>
        /// <param name="email">The email for the user.</param>
        /// <param name="userRole">The role for the user.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public UserSessionData(int userId, string email, UserRole userRole)
        {
            #region Checks

            if (userId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), $"The value of parameter '{userId}' can't be negative.");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException(nameof(email), $"The value of parameter '{email}' can't be empty.");
            }

            #endregion

            UserId = userId;
            UserEmail = email;
            UserRole = userRole;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The email for the user.
        /// </summary>
        public string UserEmail { get; } = "";

        /// <summary>
        /// The ID for the user.
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// The user role for the user. 
        /// </summary>
        public UserRole UserRole { get; }

        #endregion
    }
}

namespace FribergCarRentalsBravo.Sessions
{
    /// <summary>
    /// A helper class to handle user session data. 
    /// </summary>
    public static class UserSessionHandler
    {
        #region Methods

        /// <summary>
        /// Retrieves user data from a session. 
        /// </summary>
        /// <param name="session">The session to retrieve user data from.</param>
        /// <returns>A <see cref="UserSessionData"/> object containing the retrieved data.</returns>
        /// <remarks>If the user data was not found an <see cref="InvalidOperationException"/> will be thrown.</remarks>
        /// <exception cref="InvalidOperationException"></exception>
        public static UserSessionData GetUserData(ISession session)
        {
            return new UserSessionData(
                session.GetInt32(nameof(UserSessionData.UserId)) ?? throw new InvalidOperationException("The user ID was not found in the session variable."),
                session.GetString(nameof(UserSessionData.UserEmail)) ?? throw new InvalidOperationException("The user email was not found in the session variable."),
                (UserRole)session.GetInt32(nameof(UserSessionData.UserRole))!) ?? throw new InvalidOperationException("The user role was not found in the session variable.");
        }

        /// <summary>
        /// Sets user data to a session.
        /// </summary>
        /// <param name="session">The session to store user data in.</param>
        /// <param name="data">The user data to store.</param>
        public static void SetUserData(ISession session, UserSessionData data)
        {
            session.SetInt32(nameof(UserSessionData.UserId), data.UserId);
            session.SetString(nameof(UserSessionData.UserEmail), data.UserEmail);
            session.SetInt32(nameof(UserSessionData.UserRole), (int)data.UserRole);
        }

        /// <summary>
        /// Removes user data from a session.
        /// </summary>
        /// <param name="session">The session to remove user data from.</param>
        public static void RemoveUserData(ISession session)
        {
            session.Remove(nameof(UserSessionData.UserId));
            session.Remove(nameof(UserSessionData.UserEmail));
            session.Remove(nameof(UserSessionData.UserRole));
        }

        /// <summary>
        /// Checks whether a user with a specific role is logged in.
        /// A logged in user have user data stored in the session.
        /// </summary>
        /// <param name="session">The session that contains the user data.</param>
        /// <param name="userRole">The target role for the user.</param>
        /// <returns>True if there was a logged in user matching the user role.</returns>
        private static bool IsUserLoggedIn(ISession session, UserRole userRole)
        {
            List<string> keys = new()
            {
                nameof(UserSessionData.UserId),
                nameof(UserSessionData.UserEmail),
                nameof(UserSessionData.UserRole)
            };

            if (session.Keys.Intersect(keys).Count() == keys.Count)
            {
                return (UserRole)session.GetInt32(nameof(UserSessionData.UserRole))! == userRole;
            }

            return false;
        }

        /// <summary>
        /// Checks whether a customer is logged in. 
        /// </summary>
        /// <param name="session">The session that contains the user data.</param>
        /// <returns>True if the there was a logged in user matching the customer role.</returns>
        public static bool IsCustomerLoggedIn(ISession session)
        {
            return IsUserLoggedIn(session, UserRole.Customer);
        }

        /// <summary>
        /// Checks whether an admin is logged in. 
        /// </summary>
        /// <param name="session">The session that contains the user data.</param>
        /// <returns>True if the there was a logged in user matching the admin role.</returns>
        public static bool IsAdminLoggedIn(ISession session)
        {
            return IsUserLoggedIn(session, UserRole.Admin); 
        }

        #endregion
    }
}

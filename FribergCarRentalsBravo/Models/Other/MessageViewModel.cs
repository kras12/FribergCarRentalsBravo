using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FribergCarRentalsBravo.Models.Other
{
    #region Enums

    /// <summary>
    /// Types of messages.
    /// </summary>
    public enum MessageType
    {
        Neutral,
        Error,
        Success,
        Warning
    }

    #endregion

    #region Classes

    /// <summary>
    /// A view model for messages that can be shown to the user.
    /// </summary>
    public class MessageViewModel
    {        
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="type">The type of the message.</param>
        /// <param name="body">The body of the message.</param>
        /// <param name="title">The optional title of the message. </param>
        /// <exception cref="ArgumentException"></exception>
        public MessageViewModel(MessageType type, string body, string? title = null)
        {
            #region Checks

            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentException($"The value of parameter '{nameof(body)}' can't be empty.", nameof(body));
            }

            #endregion

            MessageType = type;
            Body = body;
            Title = title;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The body of the message. 
        /// </summary>
        [BindNever]
        public string Body { get; } = "";

        /// <summary>
        /// Returns true if the message have a title.
        /// </summary>
        [BindNever]
        public bool HaveTitle
        {
            get
            {
                return !string.IsNullOrEmpty(Title);
            }
        }

        /// <summary>
        /// The type of the message.
        /// </summary>
        [BindNever]
        public MessageType MessageType { get; }

        /// <summary>
        /// The title of the message. 
        /// </summary>
        [BindNever]
        public string? Title { get; } = null;

        #endregion
    }

    #endregion
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FribergCarRentalsBravo.Attributes
{
    /// <summary>
    /// A custom attribute class to help perform server side regular expression tests without having 
    /// the faulty client side unobtrusive jQuery scripts failing to do their part. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ServerSideRegularExpressionAttribute : ValidationAttribute
    {
        #region Fields

        /// <summary>
        /// The regular expression pattern to use. 
        /// </summary>
        private readonly string _pattern = "";

        /// <summary>
        /// The Regex object to use for evaluating the values. 
        /// </summary>
        private Regex? _regex;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor that accepts the regular expression pattern
        /// </summary>
        /// <param name="pattern">The regular expression to use.  It cannot be null.</param>
        public ServerSideRegularExpressionAttribute([StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
            : base()
        {
            _regex = new Regex(pattern);
            _pattern = pattern;
        }

        #endregion

        #region Methods        

        /// <summary>
        /// Override of <see cref="ValidationAttribute.IsValid(object)" />
        /// </summary>
        /// <remarks>
        /// This override performs the specific regular expression matching of the given <paramref name="value" />
        /// </remarks>
        /// <param name="value">The value to test for validity.</param>
        /// <returns><c>true</c> if the given value matches the current regular expression pattern</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is ill-formed.</exception>
        /// <exception cref="ArgumentException"> is thrown if the <see cref="Pattern" /> is not a valid regular expression.</exception>
        public override bool IsValid(object? value)
        {
            string? stringValue = Convert.ToString(value, CultureInfo.CurrentCulture);

            // Automatically pass if value is null or empty. RequiredAttribute should be used to assert a value is not empty.
            if (string.IsNullOrEmpty(stringValue))
            {
                return true;
            }

            foreach (ValueMatch match in _regex!.EnumerateMatches(stringValue))
            {
                // We are looking for an exact match, not just a search hit. This matches what
                // the RegularExpressionValidator control does
                return match.Index == 0 && match.Length == stringValue.Length;
            }

            return false;
        }

        #endregion
    }
}

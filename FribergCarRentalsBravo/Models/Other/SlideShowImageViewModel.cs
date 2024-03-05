using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.Data;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentalsBravo.Components;

namespace FribergCarRentalsBravo.Models.Other
{
	/// <summary>
	/// A view model class designed to be used with the image slideshow component <see cref="CarImageSlideShow"/>.
	/// </summary>
	public class SlideShowImageViewModel : ViewModelBase
    {
        #region Constructors

		/// <summary>
		/// A constructor
		/// </summary>
		/// <param name="url">The url for the image.</param>
		/// <param name="fileName">The filename for the image.</param>
		/// <param name="imageId">The ID for the image.</param>
		/// <param name="linksToAction">An optional link to another action.</param>
		/// <param name="imageCaption">An optional image caption.</param>
		public SlideShowImageViewModel(string url, string fileName = "", int? imageId = null, string? imageCaption = null, RedirectToActionData? linksToAction = null)
        {
            FileName = fileName;
            Url = url;
            ImageId = imageId;
            LinksToAction = linksToAction;
            ImageCaption = imageCaption;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The filename for the image.
        /// </summary>
        [DisplayName("File Name")]
        [BindNever]
        public string FileName { get; } = "";

        /// <summary>
        /// An optional image caption. 
        /// </summary>
        public string? ImageCaption { get; set; } = null;

        /// <summary>
        /// Returns true if there is an image caption.
        /// </summary>
        public bool HaveCaption
        {
            get
            {
                return !string.IsNullOrEmpty(ImageCaption);
            }
        }

        /// <summary>
        /// The ID for the image.
        /// </summary>
        [DisplayName("Image ID")]
        [BindNever]
        public int? ImageId { get; }

		/// <summary>
		/// An optional link to another page.
		/// </summary>
		[DisplayName("Link")]
		[BindNever]
		public RedirectToActionData? LinksToAction { get; set;  }

        /// <summary>
        /// The url for the image.
        /// </summary>
        [DisplayName("Url")]
        [BindNever]
        public string Url { get; } = "";


        #endregion
    }
}

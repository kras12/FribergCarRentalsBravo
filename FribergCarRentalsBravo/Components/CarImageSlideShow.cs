using FribergCarRentals.Models.Other;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;
using FribergCarRentalsBravo.Controllers;
using FribergCarRentalsBravo.DataAccess.Repositories;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.Models.Other;
using FribergCarRentalsBravo.Data;
using FribergCarRentalsBravo.Helpers;
using FribergCarRentalsBravo.Controllers.Customers;

namespace FribergCarRentalsBravo.Components
{
    /// <summary>
    /// A class that handles image slideshows for cars.
    /// </summary>
    public class CarImageSlideShow : ViewComponent
    {
        #region Fields

        /// <summary>
        /// The injected car repository.
        /// </summary>
        private readonly ICarRepository _carRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRepository">Injected car repository.</param>
        public CarImageSlideShow(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The invoke method.
        /// </summary>
        /// <param name="images">The images to include in the slide show.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="IViewComponentResult"/>.</returns>
        public IViewComponentResult Invoke(List<SlideShowImageViewModel> images)
        {
            if (images is null)
            {
                throw new ArgumentNullException(nameof(images));
            }

			return View(new ListViewModel<SlideShowImageViewModel>(images));
        }

        #endregion
    }
}

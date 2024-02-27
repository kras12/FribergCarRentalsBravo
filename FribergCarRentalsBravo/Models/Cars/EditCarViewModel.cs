using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentals.Models.Other;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.Models.Cars;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace FribergCarRentalsBravo.Models.Cars
{
    /// <summary>
    /// A view model class to handle data used for editing a car. 
    /// </summary>
    public class EditCarViewModel : CarViewModelBase
    {
        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        public EditCarViewModel() : base()
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="car">The car to model.</param>
        /// <param name="categories">A collection of available car categories to choose from.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public EditCarViewModel(Car car, List<CarCategory> categories)
            : base(car.Brand, car.Color, car.Model, car.ModelYear, car.RegistrationNumber, car.RentalCostPerDay, car.IsActive)
        {
            #region Checks

            if (car.CarId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(car.CarId), $"The value of property '{nameof(car.CarId)}' can't be negative.");
            }

            #endregion

            CarId = car.CarId;
            Images = car.Images.Select(x => new CarImageViewModel(x)).ToList();
            Categories = categories.Select(x => new CarCategoryViewModel(x)).ToList();
            SelectedCategoryId = car.Category!.CarCategoryId;

            PageSubTitle = $"#{CarId} - {CarInfo}";
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the car.
        /// </summary>
        [DisplayName("ID")]
        [Required]
        public int CarId { get; set; }

        /// <summary>
        /// A collection of available car categories to choose from.
        /// </summary>
        public List<CarCategoryViewModel> Categories { get; set; } = new();

        /// <summary>
        /// The images to delete.
        /// </summary>
        [DisplayName("Delete Images")]
        public List<int>? DeleteImages { get; set; } = new();

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        [DisplayName("Images")]
        [BindNever]
        public List<CarImageViewModel> Images { get; set;  } = new();

        /// <summary>
        /// The ID of the selected category.
        /// </summary>
        public int SelectedCategoryId { get; set; }

        /// <summary>
        /// The images to upload
        /// </summary>
        [DisplayName("Upload Images")]
        public List<IFormFile>? UploadImages { get; set; } = null;

        #endregion

    }
}
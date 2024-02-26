using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentals.Models.Other;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FribergCarRentals.Models.Cars;

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
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public EditCarViewModel(DataAccess.Entities.Car car) : 
            this(car.Category!, car.Brand, car.CarId, car.Color, car.Model, car.ModelYear,
                car.RegistrationNumber, car.RentalCostPerDay, car.Images.Select(x => new CarImageViewModel(x)).ToList(), car.IsActive)
        {

        }

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="category">The category for the car.</param>
        /// <param name="brand">The brand for the car.</param>
        /// <param name="carId">The ID for the car.</param>
        /// <param name="color">The color for the car.</param>
        /// <param name="model">The model for the car.</param>
        /// <param name="modelYear">The model year for the car.</param>
        /// <param name="registrationNumber">The registration number for the car.</param>
        /// <param name="rentalCostPerDay">The rental cost per day.</param>
        /// <param name="images">The images for the car.</param>
        /// <param name="isActive">True if the car is active in the rental platform.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public EditCarViewModel(CarCategory category, string brand, int carId, string color, string model, int modelYear,
            string registrationNumber, decimal rentalCostPerDay, List<CarImageViewModel> images, bool isActive)
            : base(category, brand, color, model, modelYear, registrationNumber, rentalCostPerDay, isActive)
        {
            #region Checks

            if (carId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(carId), $"The value of parameter '{carId}' can't be negative.");
            }

            #endregion

            CarId = carId;
            Images = images;

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
        /// The images to upload
        /// </summary>
        [DisplayName("Upload Images")]
        public List<IFormFile>? UploadImages { get; set; } = null;

        #endregion

    }
}
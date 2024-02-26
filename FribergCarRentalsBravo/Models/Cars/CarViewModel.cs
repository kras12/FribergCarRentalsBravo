using FribergCarRentals.Models.Cars;
using FribergCarRentalsBravo.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentalsBravo.Models.Cars
{
    /// <summary>
    /// A view model class that handles car data.
    /// </summary>
    public class CarViewModel : CarViewModelBase
    {

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="car">The car to model.</param>
        public CarViewModel(Car car) :
            this(car.Category, car.Brand, car.CarId, car.Color, car.Model, car.ModelYear, 
                car.RegistrationNumber, car.RentalCostPerDay, 
                car.Images.Select(x => new CarImageViewModel(x)).ToList(), car.IsActive) 
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
        /// <param name="propulsionSystem">The propulsion system for the car.</param>
        /// <param name="registrationNumber">The registration number for the car.</param>
        /// <param name="rentalCostPerDay">The rental cost per day.</param>
        /// <param name="rentalStatus">The rental status for the car.</param>
        /// <param name="images">The images for the car.</param>
        /// <param name="isActive">True if the car is active in the rental platform.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public CarViewModel(CarCategory category, string brand, int carId, string color, string model, int modelYear, 
            string registrationNumber, decimal rentalCostPerDay, List<CarImageViewModel> images, bool isActive)
            : base(category, brand, color, model, modelYear, registrationNumber, rentalCostPerDay, isActive)
        {
            #region Checks

            if (carId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(carId), $"The value of parameter '{carId}' can't be negative.");
            }

            if (images is null)
            {
                throw new ArgumentNullException(nameof(images), $"The value of parameter '{images}' can't be null.");
            }

            #endregion

            Category = new CarCategoryViewModel(category);
            CarId = carId;
            Images = images;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The ID for the car.
        /// </summary>
        [DisplayName("ID")]
        [BindNever]
        public int CarId { get; }

        /// <summary>
        /// The category for the car.
        /// </summary>
        [DisplayName("Category")]
        public virtual CarCategoryViewModel Category { get; set; }

        /// <summary>
        /// A collection of images for the car.
        /// </summary>
        [DisplayName("Images")]
        [BindNever]
        public List<CarImageViewModel> Images { get; } = new();

        /// <summary>
        /// A text that shows whether the car is active. 
        /// </summary>
        [DisplayName("Is Active")]
        public string IsActiveText
        {
            get
            {
                return IsActive ? "Active" : "Inactive";
            }
        }

        /// <summary>
        /// The rental cost per day.
        /// </summary>
        [DisplayName("Cost per day")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = DefaultPriceOutputFormatString)]
        [BindNever]
        public override decimal RentalCostPerDay { get; set; }

        #endregion
    }
}
using FribergCarRentalsBravo.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentalsBravo.DataAccess.Repositories
{
    /// <summary>
    /// An interface for a car repository.
    /// </summary>
    public interface ICarRepository
    {
        #region Methods

        /// <summary>
        /// Adds a car to the database.
        /// </summary>
        /// <param name="car">The car to add.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public Task AddAsync(Car car);

        /// <summary>
        /// Deletes a car from the database.
        /// </summary>
        /// <param name="car">The car to delete.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public Task DeleteAsync(Car car);

        /// <summary>
        /// Deletes a car from the database.
        /// </summary>
        /// <param name="id">The ID of the car to delete.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task DeleteAsync(int id);

        /// <summary>
        /// Performs a search in the database and returns found cars.
        /// </summary>
        /// <remarks>Returned cars will not be tracked by EF Core.</remarks>
        /// <param name="predicate">The predicate to use.</param>
        /// <returns>A <see cref="Task"/> object containing a collection of the resulting cars.</returns>
        public Task<IEnumerable<Car>> FindAsync(Expression<Func<Car, bool>> predicate);

        /// <summary>
        /// Gets all cars from the database.
        /// </summary>
        /// <remarks>Returned cars will not be tracked by EF Core.</remarks>
        /// <returns>A <see cref="Task"/> object containg a collection of all cars found.</returns>
        public Task<IEnumerable<Car>> GetAllAsync();

        /// <summary>
        /// Gets a car by ID.
        /// </summary>
        /// <remarks>Returned cars will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the car.</param>
        /// <returns>A <see cref="Task"/> object containg the car.</returns>
        public Task<Car?> GetByIdAsync(int id);

        /// <summary>
        /// Attempts to fetch all images for a car with a specific ID. 
        /// </summary>
        /// <remarks>Returned cars will not be tracked by EF Core.</remarks>
        /// <param name="id">The ID of the car.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing a collection containing the images found.</returns>
        public Task<IEnumerable<CarImage>> GetCarImagesAsync(int id);

        /// <summary>
        /// Returns all the cars that matches the specified category and that are available to be rented out within the desired timespan. 
        /// </summary>
        /// <param name="pickupDate">The pickup date for the car.</param>
        /// <remarks>Returned cars will not be tracked by EF Core.</remarks>
        /// <param name="category">The category of the car.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing a collection of matching cars.</returns>
        public Task<IEnumerable<Car>> GetRentableCarsAsync(DateTime pickupDate, CarCategory? category = null);

        /// <summary>
        /// Updates a car in the database.
        /// </summary>
        /// <param name="car">The car to update.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public Task UpdateAsync(Car car);

        Task<int> GetAmountOfCarsAsync();

        #endregion
    }
}

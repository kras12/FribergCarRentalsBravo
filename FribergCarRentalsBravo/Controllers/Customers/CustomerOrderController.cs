using FribergCarRentals.Models.Order;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.DataAccess.Repositories;
using FribergCarRentalsBravo.Helpers;
using FribergCarRentalsBravo.Models.Cars;
using FribergCarRentalsBravo.Sessions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FribergCarRentalsBravo.Controllers.Customers
{
    public class CustomerOrderController : Controller
    {
        #region Constants

        /// <summary>
        /// The key for the created order flag stored in temp storage.
        /// </summary>
        public const string IsNewOrderTempDataKey = "CustomerOrderControllerIsNewOrder";

        #endregion

        #region Fields

        /// <summary>
        /// The injected car repository.
        /// </summary>
        public ICarRepository _carRepository { get; }

        /// <summary>
        /// The injected car category repository.
        /// </summary>
        public ICarCategoryRepository _carCategoryRepository { get; }

        /// <summary>
        /// The injected customer repository.
        /// </summary>
        public ICustomerRepository _customerRepository { get; }

        /// <summary>
        /// The injected order repository.
        /// </summary>
        public IOrderRepository _orderRepository { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="carRepository">The injected car repository.</param>
        /// /// <param name="carCategoryRepository">The injected car category repository.</param>
        /// <param name="customerRepository">he injected customer repository.</param>
        public CustomerOrderController(ICarRepository carRepository, ICarCategoryRepository carCategoryRepository, ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            this._carRepository = carRepository;
            this._carCategoryRepository = carCategoryRepository;
            this._customerRepository = customerRepository;
            this._orderRepository = orderRepository;
        }

        #endregion

        #region Actions

        // GET: CustomerOrderController/Book
        public async Task<IActionResult> BookAsync()
        {            
            return View(new BookCarViewModel(availableCarCategoryFilters: (await _carCategoryRepository.GetAllAsync()).ToList(), havePerformedCarSearch: false));
        }

        // POST: CustomerOrderController/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(BookCarViewModel bookCarViewModel)
        {
            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (bookCarViewModel.PickupDateFilter is null || bookCarViewModel.PickupDateFilter <= DateTime.Now)
                {
                    ModelState.AddModelError($"{nameof(BookCarViewModel)}.{nameof(BookCarViewModel.PickupDateFilter)}",
                        "The pickup date must be at least one day into the future.");
                    return View(bookCarViewModel);
                }
                else if (bookCarViewModel.ReturnDateFilter is null || bookCarViewModel.ReturnDateFilter.Value.Date < bookCarViewModel.PickupDateFilter.Value.Date)
                {
                    ModelState.AddModelError($"{nameof(BookCarViewModel)}.{nameof(BookCarViewModel.ReturnDateFilter)}",
                        "The return date can't occur before the pickup date.");
                    return View(bookCarViewModel);
                }

                CarCategory? carCategoryFilter = null; 
                
                if (bookCarViewModel.SelectedCarCategoryFilter > 0)
                {
                    carCategoryFilter = await _carCategoryRepository.GetByIdAsync(bookCarViewModel.SelectedCarCategoryFilter);

                    if (carCategoryFilter is null)
                    {
                        throw new Exception("Failed to find the car category");
                    }
                }

                var cars = await _carRepository.GetRentableCarsAsync(bookCarViewModel.PickupDateFilter.Value, carCategoryFilter);

                return View(new BookCarViewModel(
                    availableCarCategoryFilters: (await _carCategoryRepository.GetAllAsync()).ToList(),
                    havePerformedCarSearch: true,
                    availableCars: (await _carRepository.GetAllAsync()).ToList(),
                    pickupDateFilter: bookCarViewModel.PickupDateFilter,
                    returnDateFilter: bookCarViewModel.ReturnDateFilter,
                    carCategoryFilter: bookCarViewModel.SelectedCarCategoryFilter));
            }

            return View(bookCarViewModel);
        }

        // GET: CustomerOrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomerOrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerOrderController/Book
        [HttpPost("{carId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderViewModel createOrderViewModel)
        {
            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (createOrderViewModel.PickupDate.Date < DateTime.Now.Date)
                {
                    throw new Exception("The car pickup date can't be in the past.");
                }
                else if (createOrderViewModel.ReturnDate.Date < createOrderViewModel.PickupDate.Date)
                {
                    throw new Exception("The return date can't occur before the pickup date.");
                }

                var customer = await _customerRepository.GetCustomerById(createOrderViewModel.CustomerId);
                var car = await _carRepository.GetByIdAsync(createOrderViewModel.CarId);

                if (customer is not null && car is not null)
                {
                    var order = new Order(DateTime.Now, car, customer, createOrderViewModel.PickupDate, createOrderViewModel.ReturnDate, car.RentalCostPerDay);
                    await _orderRepository.AddAsync(order);
                    TempDataHelper.Set(TempData, IsNewOrderTempDataKey, true);
                    return RedirectToAction(nameof(Details), new { id = order.OrderId });
                }

                throw new Exception($"Failed to retrieve car and/or customer from the database. - CarID: {createOrderViewModel.CarId} - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId}");
            }

            throw new Exception($"Failed to create an order for the car with id: {createOrderViewModel.CarId} - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // POST: CustomerOrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerOrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerOrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        #endregion
    }
}

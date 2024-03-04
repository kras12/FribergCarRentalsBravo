﻿using FribergCarRentals.Models.Orders;
using FribergCarRentalsBravo.Controllers.Admin;
using FribergCarRentalsBravo.Data;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.DataAccess.Repositories;
using FribergCarRentalsBravo.Helpers;
using FribergCarRentalsBravo.Models.Admin;
using FribergCarRentalsBravo.Models.Cars;
using FribergCarRentalsBravo.Models.Orders;
using FribergCarRentalsBravo.Sessions;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FribergCarRentalsBravo.Controllers.Customers
{
    public class CustomerOrderController : Controller
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the order that was canceled.
        /// </summary>
        public const string CanceledOrderIdTempDataKey = "CustomerCanceledOrderId";

        /// <summary>
        /// The key for the canceled order redirect data stored in temp storage.
        /// </summary>
        public const string CanceledOrderRedirectToPageTempDataKey = "CustomerCanceledOrderRedirectToPage";

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

        // GET: CustomerOrderController/BookAsync
        public async Task<IActionResult> Book()
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Book));
            }

            return View(new BookCarViewModel(availableCarCategoryFilters: (await _carCategoryRepository.GetAllAsync()).ToList(), havePerformedCarSearch: false));
        }

        // POST: CustomerOrderController/BookAsync
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(BookCarViewModel bookCarViewModel)
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Book));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (bookCarViewModel.PickupDate is null || bookCarViewModel.PickupDate <= DateTime.Now)
                {
                    ModelState.AddModelError($"{nameof(BookCarViewModel.PickupDate)}",
                        "The pickup date must be at least one day into the future.");
                }
                else if (bookCarViewModel.ReturnDate is null || bookCarViewModel.ReturnDate.Value.Date < bookCarViewModel.PickupDate.Value.Date)
                {
                    ModelState.AddModelError($"{nameof(BookCarViewModel.ReturnDate)}",
                        "The return date can't occur before the pickup date.");
                }
                else
                {
                    CarCategory? carCategoryFilter = null;

                    if (bookCarViewModel.SelectedCarCategoryFilter > 0)
                    {
                        carCategoryFilter = await _carCategoryRepository.GetByIdAsync(bookCarViewModel.SelectedCarCategoryFilter);

                        if (carCategoryFilter is null)
                        {
                            throw new Exception("Failed to find the car category");
                        }
                    }

                    var cars = (await _carRepository.GetRentableCarsAsync(bookCarViewModel.PickupDate.Value, carCategoryFilter)).ToList();

                    return View(new BookCarViewModel(
                        availableCarCategoryFilters: (await _carCategoryRepository.GetAllAsync()).ToList(),
                        havePerformedCarSearch: true,
                        availableCars: cars,
                        pickupDateFilter: bookCarViewModel.PickupDate,
                        returnDateFilter: bookCarViewModel.ReturnDate,
                        carCategoryFilter: bookCarViewModel.SelectedCarCategoryFilter));
                }                
            }

            bookCarViewModel.SetAvailableCarCategoryFilters(await _carCategoryRepository.GetAllAsync());
            return View(bookCarViewModel);
        }

        // GET: CustomerOrderController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Details), id);
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var order = await _orderRepository.GetOrderByIdAsync(id);

                if (order is not null)
                {
                    TempDataHelper.TryGet(TempData, IsNewOrderTempDataKey, out bool orderWasCreated);
                    return View(new OrderViewModel(order, isNewOrder: orderWasCreated));
                }
            }

            throw new Exception("Failed to find customer order.");            
        }

        // POST: CustomerOrderController/Cancel/(5)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Cancel), id);
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (await _orderRepository.TryCancelOrderAsync(id))
                {
                    TempDataHelper.Set(TempData, CanceledOrderIdTempDataKey, id);

                    if (TempDataHelper.TryGet(TempData, CanceledOrderRedirectToPageTempDataKey, out RedirectToActionData? data))
                    {
                        return RedirectToAction(data.Action, data.Controller, data.RouteValues);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Details), new { id = id });
                    }
                }

                throw new Exception($"Failed to cancel order with id: {id} - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId}");
            }

            throw new Exception($"Model validation failed: CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // POST: CustomerOrderController/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCarViewModel bookCarViewModel)
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Book));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (bookCarViewModel.PickupDate is null || bookCarViewModel.PickupDate <= DateTime.Now)
                {
                    throw new Exception("The pickup date must be at least one day into the future.");
                }
                else if (bookCarViewModel.ReturnDate is null || bookCarViewModel.ReturnDate.Value.Date < bookCarViewModel.PickupDate.Value.Date)
                {
                    throw new Exception("The return date can't occur before the pickup date.");
                }
                else
                {
                    var customer = await _customerRepository.GetCustomerById(UserSessionHandler.GetUserData(HttpContext.Session).UserId);
                    var car = await _carRepository.GetByIdAsync(bookCarViewModel.CarId);

                    if (customer is not null && car is not null)
                    {
                        var order = new Order(DateTime.Now, car, customer, bookCarViewModel.PickupDate.Value, bookCarViewModel.ReturnDate.Value, car.RentalCostPerDay);
                        await _orderRepository.CreateOrderAsync(order);
                        TempDataHelper.Set(TempData, IsNewOrderTempDataKey, true);
                        return RedirectToAction(nameof(Details), new { id = order.OrderId });
                    }

                    throw new Exception($"Failed to retrieve car and/or customer from the database. - CarID: {bookCarViewModel.CarId} - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId}");
                }               
            }

            throw new Exception($"Failed to create an order for the car with id: {bookCarViewModel.CarId} - CustomerID: {UserSessionHandler.GetUserData(HttpContext.Session).UserId} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
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

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="action">The action to redirect to.</param>
        /// <param name="id">An optional ID for the order.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string action, int? id = null)
        {
            RouteValueDictionary? routeValues = id is not null ? new RouteValueDictionary(new { id = id }) : null;

            TempDataHelper.Set(TempData, CustomerController.RedirectToPageTempDataKey, new RedirectToActionData(
                    action, ControllerHelper.GetControllerName<CustomerOrderController>(), routeValues: routeValues));

            return RedirectToAction(nameof(CustomerController.Authenticate), ControllerHelper.GetControllerName<CustomerController>());
        }

        #endregion
    }
}
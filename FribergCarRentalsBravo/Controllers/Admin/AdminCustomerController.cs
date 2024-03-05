using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.Data;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.DataAccess.Repositories;
using FribergCarRentalsBravo.Helpers;
using FribergCarRentalsBravo.Models.Cars;
using FribergCarRentalsBravo.Models.Customers;
using FribergCarRentalsBravo.Sessions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentalsBravo.Controllers.Admin
{
    [Route($"Admin/Customers/[action]")]
    public class AdminCustomerController : Controller
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the customer that was created.
        /// </summary>
        public const string CreatedCustomerIdTempDataKey = "AdminCreatedCustomerId";

        #endregion

        #region Fields

        public ICustomerRepository customerRep { get; }

        #endregion

        #region Constructors

        public AdminCustomerController(ICustomerRepository customerRep)
        {
            this.customerRep = customerRep;
        }

        #endregion

        #region Actions

        // GET: CustomerController
        public async Task<IActionResult> Index()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Index));
            }

            return View(new ListViewModel<CustomerViewModel>((await customerRep.GetAllCustomers()).Select(x => new CustomerViewModel(x)).ToList()));
        }

        // GET: CustomerController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Details));
            }

            var customer = await customerRep.GetCustomerById(id);

            if (customer == null)
            {
                throw new Exception($"Failed to find customer with id '{id}'.");
            }

            return View(new CustomerViewModel(customer));
        }

        // GET: CustomerController/Create
        public IActionResult Create()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Create));
            }

            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterCustomerViewModel registerCustomerViewModel)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Create));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!DataTransferHelper.TryTransferData(registerCustomerViewModel, out Customer customer))
                {
                    throw new Exception("Failed to transfer data from the view model to the entity");
                }

                await customerRep.CreateCustomer(customer);
                TempDataHelper.Set(TempData, CreatedCustomerIdTempDataKey, customer.CustomerId);
                return RedirectToAction(nameof(Details), new { id = customer.CustomerId });
            }

            return View(registerCustomerViewModel);
        }

        // GET: CustomerController/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Edit));
            }

            if (id <= 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            Customer customer = await customerRep.GetCustomerById(id);

            if (customer is not null)
            {
                EditCustomerViewModel viewModel = new EditCustomerViewModel(customer);
                return View(viewModel);
            }

            throw new Exception($"Failed to find the customer with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // POST: CustomerController/Edit/5
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCustomerViewModel editCustomerViewModel)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Edit));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!DataTransferHelper.TryTransferData(editCustomerViewModel, out Customer customer))
                {
                    throw new Exception("Failed to transfer data from view model to entity.");
                }

                await customerRep.EditCustomer(customer);
                // TODO - Create message
                return View(editCustomerViewModel);
            }

            return View(editCustomerViewModel);
        }

        // POST: CustomerController/Delete/5             

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Delete));
            }

            if (id <= 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            await customerRep.DeleteCustomerByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="action">The action to redirect to.</param>
        /// <param name="id">An optional ID for the customer.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string action, int? id = null)
        {
            RouteValueDictionary? routeValues = id is not null ? new RouteValueDictionary(new { id = id }) : null;

            TempDataHelper.Set(TempData, AdminController.RedirectToPageTempDataKey, new RedirectToActionData(
                    action, ControllerHelper.GetControllerName<AdminCustomerController>(), routeValues: routeValues));

            return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
        }

        #endregion
    }
}
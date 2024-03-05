using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FribergCarRentalsBravo.DataAccess.DatabaseContexts;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.DataAccess.Repositories;
using FribergCarRentalsBravo.Data;
using FribergCarRentalsBravo.Helpers;
using FribergCarRentalsBravo.Sessions;
using FribergCarRentalsBravo.Models.Customers;

namespace FribergCarRentalsBravo.Controllers.Customers
{
    [Route($"Customer/[action]")]
    public class CustomerController : Controller
    {
        #region Constants

        /// <summary>
        /// The key for the redirection data for the page to redirect to after logins. 
        /// </summary>
        public const string RedirectToPageTempDataKey = "CustomerLoginRedirectToPage";

        #endregion

        #region Fields

        public ICustomerRepository customerRep { get; }

        #endregion

        #region Constructors

        public CustomerController(ICustomerRepository customerRep)
        {
            this.customerRep = customerRep;
        }

        #endregion

        #region Actions

        // GET: CustomerController
        [HttpGet]
        public ActionResult Authenticate()
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return TempDataOrHomeRedirect();
            }

            return View(new RegisterOrLoginCustomerViewModel());
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterCustomerViewModel registerCustomerViewModel)
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return TempDataOrHomeRedirect();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!DataTransferHelper.TryTransferData(registerCustomerViewModel, out Customer customer))
                {
                    throw new Exception("Failed to transfer data from the view model to the entity.");
                }

                if (await customerRep.CustomerExists(customer.Email))
                {
                    // The key needs to be the name of the view model (instead of an empty string) because the error is shown in a partial view. 
                    ModelState.AddModelError(nameof(RegisterCustomerViewModel), "An account already exists with that email.");
                }
                else
                {
                    await customerRep.CreateCustomer(customer);
                    LoginCustomer(customer);
                    return TempDataOrHomeRedirect();
                }                
            }

            return View(nameof(Authenticate), new RegisterOrLoginCustomerViewModel() { RegisterCustomerViewModel = registerCustomerViewModel });
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Index));
            }

            if (id <= 0 || UserSessionHandler.GetUserData(HttpContext.Session).UserId != id)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var customer = await customerRep.GetCustomerById(id);

                if (customer != null)
                {
                    await customerRep.DeleteCustomer(customer);
                    return Logout();
                }
            }            

            throw new Exception($"Failed to delete the customer with id: {id}");
        }

        // GET: CustomerController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Details), id);
            }

            var customer = await customerRep.GetCustomerById(id);

            if (customer == null)
            {
                throw new Exception($"Failed to find the customer with id: {id}");
            }

            return View(new CustomerViewModel(customer));
        }

        // GET: CustomerController/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Edit), id);
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            var customer = await customerRep.GetCustomerById(id);

            if (customer == null)
            {
                throw new Exception($"Failed to find the customer with id: {id}");
            }

            return View(new EditCustomerViewModel(customer));
        }

        // POST: CustomerController/Edit/5
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCustomerViewModel editCustomerViewModel)
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Index));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!DataTransferHelper.TryTransferData(editCustomerViewModel, out Customer customer))
                {
                    throw new Exception("Failed to transfer data from view model to entity.");
                }
                
                await customerRep.EditCustomer(customer);
                // TODO - Add user message
                return View(editCustomerViewModel);
            }

                return View(editCustomerViewModel);
            }

        // GET: CustomerController
        public async Task<IActionResult> Index()
        {
            if (!UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Index));
            }

            var customerId = UserSessionHandler.GetUserData(HttpContext.Session).UserId;
            var customer = await customerRep.GetCustomerById(customerId);

            if (customer == null)
            {
                throw new Exception($"Failed to find the customer with id: {customerId}");
            }

            return View(new CustomerViewModel(customer));
        }

        // Post: CustomerController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginCustomerViewModel loginCustomerViewModel)
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                return TempDataOrHomeRedirect();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var customer = await customerRep.GetMatchingCustomerAsync(loginCustomerViewModel.Email, loginCustomerViewModel.Password);

                if (customer is null)
                {
                    // The key needs to be the name of the view model (insted of empty string) because the error is shown in a partial view. 
                    ModelState.AddModelError(nameof(LoginCustomerViewModel), "No account matched the entered email/password.");
                }
                else
                {
                    LoginCustomer(customer);
                    return TempDataOrHomeRedirect();
                }
            }

            return View(nameof(Authenticate), new RegisterOrLoginCustomerViewModel() { LoginCustomerViewModel = loginCustomerViewModel });
        }

        // GET: CustomerController
        [HttpGet]
        public ActionResult Logout()
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                UserSessionHandler.RemoveUserData(HttpContext.Session);
            }

            return RedirectToAction(nameof(HomeController.Index), ControllerHelper.GetControllerName<HomeController>());
        }
        #endregion

        #region Methods

        /// <summary>
        /// Saves the customer user data in the session storage. 
        /// </summary>
        /// <param name="customer">The customer to login.</param>
        [NonAction]
        private void LoginCustomer(Customer customer)
        {
            UserSessionHandler.SetUserData(HttpContext.Session,
                    new UserSessionData(customer.CustomerId, customer.Email, UserRole.Customer));
        }

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="action">The action to redirect to.</param>
        /// <param name="id">An optional ID for the customer.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string action, int? id = null)
        {
            RouteValueDictionary? routeValues = id is not null ? new RouteValueDictionary(new { id = id }) : null;

            TempDataHelper.Set(TempData, RedirectToPageTempDataKey, new RedirectToActionData(
                    action, ControllerHelper.GetControllerName<CustomerController>(), routeValues: routeValues));

            return RedirectToAction(nameof(Authenticate), ControllerHelper.GetControllerName<CustomerController>());
        }

        /// <summary>
        /// Redirects the customer to the page stored in the temp storage if such data exists, else redirects the customer to the homepage. 
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        [NonAction]
        private ActionResult TempDataOrHomeRedirect()
        {
            if (TempDataHelper.TryGet<RedirectToActionData>(TempData, RedirectToPageTempDataKey, out var data))
            {
                return RedirectToAction(data.Action, data.Controller, data.RouteValues);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}

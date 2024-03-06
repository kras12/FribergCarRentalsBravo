using FribergCarRentals.Models.Cars;
using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.Data;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.DataAccess.Repositories;
using FribergCarRentalsBravo.Helpers;
using FribergCarRentalsBravo.Models.Admin;
using FribergCarRentalsBravo.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentalsBravo.Controllers.Admin
{
    public class AdminController : Controller
    {
        #region Constants

        /// <summary>
        /// The key for the redirection data for the page to redirect to after logins. 
        /// </summary>
        public const string RedirectToPageTempDataKey = "AdminLoginRedirectToPage";
        

        #endregion

        #region Fields

        /// <summary>
        /// The injected admin repository.
        /// </summary>
        public IAdminRepository adminRep { get; }
        private readonly ICustomerRepository customerRep;
        private readonly ICarRepository carRep;
        private readonly ICarCategoryRepository carCategoryRep;
        private readonly IOrderRepository orderRep;

        #endregion

        #region Constructors

        public AdminController(IAdminRepository adminRep, 
                              ICustomerRepository customerRep, 
                              ICarRepository carRep,
                              ICarCategoryRepository carCategoryRep,
                              IOrderRepository orderRep)
        {
            this.adminRep = adminRep;
            this.customerRep = customerRep;
            this.carRep = carRep;
            this.carCategoryRep = carCategoryRep;
            this.orderRep = orderRep;
        }

        #endregion

        #region Actions

        // GET: AdminController
        public async Task<IActionResult> Index()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Index));
            }

            var userData = UserSessionHandler.GetUserData(HttpContext.Session);
            var admin = await adminRep.GetAdminByIdAsync(userData.UserId);

            if (admin is not null)
            {
                var customerAmount = await customerRep.GetAmountOfCustomersAsync();
                ViewBag.CustomerAmount = customerAmount.ToString();

                var carAmount = await carRep.GetAmountOfCarsAsync();
                ViewBag.CarAmount = carAmount.ToString();

                var carCategoryAmount = await carCategoryRep.GetAmountOfCarCategoriesAsync();
                ViewBag.CarCategoryAmount = carCategoryAmount.ToString();

                var orderAmount = await orderRep.GetAmountOfOrdersAsync();
                ViewBag.OrderAmount = orderAmount.ToString();

                return View(new AdminViewModel(admin));
            }

            throw new Exception("Failed to find the admin in the database.");
        }

        // GET: AdminController
        public ActionResult Login()
        {
            if (UserSessionHandler.IsCustomerLoggedIn(HttpContext.Session))
            {
                UserSessionHandler.RemoveUserData(HttpContext.Session);
            }

            if (UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return TempDataOrHomeRedirect();
            }

            LoginAdminViewModel viewModel = new();
            return View(viewModel);
        }

        // Post: AdminController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginAdminViewModel loginAdminViewModel)
        {
            if (UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return TempDataOrHomeRedirect();
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var admin = await adminRep.GetMatchingAdminAsync(loginAdminViewModel.Email, loginAdminViewModel.Password);

                if (admin is null)
                {
                    ModelState.AddModelError("", "No account matched the entered email/password.");
                    return View(loginAdminViewModel);
                }
                else
                {
                    LoginAdmin(admin);
                    return TempDataOrHomeRedirect();
                }
            }

            return View(loginAdminViewModel);
        }

        // GET: AdminController
        public ActionResult Logout()
        {
            if (UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                UserSessionHandler.RemoveUserData(HttpContext.Session);
            }

            return TempDataOrHomeRedirect();
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Saves the admin user data in the session storage. 
        /// </summary>
        /// <param name="admin">The admin to login.</param>
        [NonAction]
        private void LoginAdmin(AdminUser admin)
        {
            UserSessionHandler.SetUserData(HttpContext.Session,
                    new UserSessionData(admin.AdminId, admin.Email, UserRole.Admin));
        }

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="action">The action to redirect to.</param>
        /// <param name="id">An optional ID for the user.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string action, int? id = null)
        {
            RouteValueDictionary? routeValues = id is not null ? new RouteValueDictionary(new { id = id }) : null;

            TempDataHelper.Set(TempData, RedirectToPageTempDataKey, new RedirectToActionData(
                    action, ControllerHelper.GetControllerName<AdminController>(), routeValues: routeValues));

            return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
        }

        /// <summary>
        /// Redirects the admin to the page stored in the temp storage if such data exists, else redirects the admin to the homepage. 
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
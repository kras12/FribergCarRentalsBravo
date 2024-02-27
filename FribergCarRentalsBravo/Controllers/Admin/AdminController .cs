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

        #endregion

        #region Constructors

        public AdminController(IAdminRepository adminRep)
        {
            this.adminRep = adminRep;
        }

        #endregion

        #region Actions

        // GET: AdminController/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Index));
            }

            return View(await adminRep.GetAdminByIdAsync(id));
        }

        // GET: AdminController/Edit/5
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(EditAsync));
            }

            var admin = await adminRep.GetAdminByIdAsync(id);

            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminUser admin)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(EditAsync));
            }

            if (id != admin.AdminId)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await adminRep.EditAsync(admin);
                }
                catch (Exception)
                {
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
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
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string action)
        {
            TempDataHelper.Set(TempData, RedirectToPageTempDataKey, new RedirectToActionData(
                    action, ControllerHelper.GetControllerName<AdminController>()));

            return RedirectToAction(nameof(Login), ControllerHelper.GetControllerName<AdminController>());
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
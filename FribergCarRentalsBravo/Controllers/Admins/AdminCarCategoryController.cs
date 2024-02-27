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

namespace FribergCarRentalsBravo.Controllers.Admins
{
    public class AdminCarCategoryController : Controller
    {
        #region Fields

        private readonly ICarCategoryRepository carCategoryRepo;

        #endregion

        #region Constructors        

        public AdminCarCategoryController(ICarCategoryRepository carCategoryRepo)
        {
            this.carCategoryRepo = carCategoryRepo;
        }

        #endregion

        #region Actions

        // GET: CarCategory
        public async Task<IActionResult> Index()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Index));
            }

            return View(await carCategoryRepo.GetAllAsync());
        }

        // GET: CarCategory/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Details));
            }

            if (id == null)
            {
                return NotFound();
            }

            var carCategory = await carCategoryRepo.GetByIdAsync(id);

            if (carCategory == null)
            {
                return NotFound();
            }

            return View(carCategory);
        }

        // GET: CarCategory/Create
        public async Task<IActionResult> Create()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Create));
            }

            return View();
        }

        // POST: CarCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarCategoryId,Name")] CarCategory carCategory)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Create));
            }

            if (ModelState.IsValid)
            {
                await carCategoryRepo.CreateNewCarCategoryAsync(carCategory);
                return RedirectToAction(nameof(Index));
            }

            return View(carCategory);
        }

        // GET: CarCategory/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Edit));
            }

            if (id == null)
            {
                return NotFound();
            }

            var carCategory = await carCategoryRepo.GetByIdAsync(id);
            if (carCategory == null)
            {
                return NotFound();
            }
            return View(carCategory);
        }

        // POST: CarCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarCategoryId,Name")] CarCategory carCategory)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Edit));
            }

            if (id != carCategory.CarCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                carCategoryRepo.UpdateCarCategoryrAsync(carCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(carCategory);
        }

        // GET: CarCategory/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Delete));
            }

            if (id == null)
            {
                return NotFound();
            }

            var carCategory = await carCategoryRepo.GetByIdAsync(id);
            if (carCategory == null)
            {
                return NotFound();
            }

            return View(carCategory);
        }

        // POST: CarCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(DeleteConfirmed));
            }

            var carCategory = await carCategoryRepo.GetByIdAsync(id);
            if (carCategory != null)
            {
                await carCategoryRepo.DeleteCarCategoryAsync(carCategory);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="action">The action to redirect to.</param>
        /// <param name="id">An optional ID for the category.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string action, int? id = null)
        {
            RouteValueDictionary? routeValues = id is not null ? new RouteValueDictionary(new { id = id }) : null;

            TempDataHelper.Set(TempData, AdminController.RedirectToPageTempDataKey, new RedirectToActionData(
                    action, ControllerHelper.GetControllerName<AdminCarCategoryController>(), routeValues: routeValues));

            return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
        }

        #endregion
    }
}

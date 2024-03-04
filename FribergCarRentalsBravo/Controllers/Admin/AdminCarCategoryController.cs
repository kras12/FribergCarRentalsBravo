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
using FribergCarRentalsBravo.Models.Cars;
using FribergCarRentals.Models.Other;
using System.Diagnostics;

namespace FribergCarRentalsBravo.Controllers.Admin
{
    [Route($"Admin/Cars/Categories/[action]")]
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

            return View(new ListViewModel<CarCategoryViewModel>((await carCategoryRepo.GetAllAsync()).Select(x => new CarCategoryViewModel(x)).ToList()));
        }

        // GET: CarCategory/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Details));
            }

            if (id <= 0)
            {
                throw new Exception("Invalid category ID.");
            }

            var carCategory = await carCategoryRepo.GetByIdAsync(id);

            if (carCategory == null)
            {
                return NotFound();
            }

            return View(new CarCategoryViewModel(carCategory));
        }

        // GET: CarCategory/Create
        public async Task<IActionResult> Create()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Create));
            }

            return View(new CreateCarCategoryViewModel());
        }

        // POST: CarCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarCategoryViewModel createCarCategoryViewModel)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Create));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!DataTransferHelper.TryTransferData(createCarCategoryViewModel, out CarCategory carCategory))
                {
                    throw new Exception("Failed to transfer data from the view model to the entity");
                }

                await carCategoryRepo.CreateNewCarCategoryAsync(carCategory);
                return RedirectToAction(nameof(Index));
            }

            return View(createCarCategoryViewModel);
        }

        // GET: CarCategory/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Edit));
            }

            var carCategory = await carCategoryRepo.GetByIdAsync(id);
            if (carCategory == null)
            {
                throw new Exception("Invalid category ID.");
            }
            return View(new EditCarCategoryViewModel(carCategory));
        }

        // POST: CarCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCarCategoryViewModel editCarCategoryViewModel)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Edit));
            }


            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!DataTransferHelper.TryTransferData(editCarCategoryViewModel, out CarCategory carCategory))
                {
                    throw new Exception("Failed to transfer data from view model to entity.");
                }

                await carCategoryRepo.UpdateCarCategoryAsync(carCategory);
                return RedirectToAction(nameof(Index));
            }

            return View(editCarCategoryViewModel);
        }

        // POST: CarCategory/Delete/5
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

            await carCategoryRepo.DeleteCarCategoryByIdAsync(id);
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

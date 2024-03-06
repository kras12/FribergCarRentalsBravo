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
using System.Runtime.ConstrainedExecution;

namespace FribergCarRentalsBravo.Controllers.Admin
{
    [Route($"Admin/Cars/Categories/[action]")]
    public class AdminCarCategoryController : Controller
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the car category that was created.
        /// </summary> 
        public const string CreatedCategoryIdTempDataKey = "AdminCreatedCategoryId";

        /// <summary>
        /// The key for the ID of the car category that was deleted.
        /// </summary>
        public const string DeletedCategoryIdTempDataKey = "AdminDeletedCategoryId";

        /// <summary>
        /// The key for the deleted car category redirect data stored in temp storage.
        /// </summary>
        public const string RedirectToPageAfterDeleteTempDataKey = "AdminDeletedCategoryRedirectToPage";

        #endregion

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

            ListViewModel<CarCategoryViewModel> viewModel = new ((await carCategoryRepo.GetAllAsync()).Select(x => new CarCategoryViewModel(x)).ToList());

            if (TempDataHelper.TryGet(TempData, CreatedCategoryIdTempDataKey, out int categoryId))
            {
                viewModel.Messages.Add(UserMesssageHelper.CreateCarCategoryCreationSuccessMessage(categoryId));
                return View(viewModel);
            }

            if (TempDataHelper.TryGet(TempData, DeletedCategoryIdTempDataKey, out int deletedCategoryId))
            {
                viewModel.Messages.Add(UserMesssageHelper.CreateCarDeletionSuccessMessage(deletedCategoryId));
            }

            return View(viewModel);
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
                TempDataHelper.Set(TempData, CreatedCategoryIdTempDataKey, carCategory.CarCategoryId);
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
        [HttpPost("{id}")]
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
                EditCarCategoryViewModel viewModel = new EditCarCategoryViewModel(carCategory);
                viewModel.Messages.Add(UserMesssageHelper.CreateCarCategoryUpdateSuccessMessage(editCarCategoryViewModel.CarCategoryId));
                return View(viewModel);
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
            TempDataHelper.Set(TempData, DeletedCategoryIdTempDataKey, id);

            if (TempDataHelper.TryGet(TempData, RedirectToPageAfterDeleteTempDataKey, out RedirectToActionData? data))
            {
                return RedirectToAction(data.Action, data.Controller, data.RouteValues);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
            
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

        /// <summary>
        /// Saves data for redirecting back to an action after a car category has been deleted. 
        /// </summary>
        /// <param name="redirectToAction">The action to redirect to.</param>
        private void SaveRedirectBackInstructionsForDeleteCarAction(string redirectToAction)
        {
            TempDataHelper.Set(TempData, RedirectToPageAfterDeleteTempDataKey, new RedirectToActionData(
                    redirectToAction, ControllerHelper.GetControllerName<AdminCarController>()));
        }

        #endregion
    }
}

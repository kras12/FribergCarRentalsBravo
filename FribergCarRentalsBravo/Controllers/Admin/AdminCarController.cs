using FribergCarRentals.Models.Cars;
using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.Data;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.DataAccess.Repositories;
using FribergCarRentalsBravo.Helpers;
using FribergCarRentalsBravo.Models.Cars;
using FribergCarRentalsBravo.Sessions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentalsBravo.Controllers.Admin
{
    [Route($"{CurrentControllerRoutePart}/[action]")]
    public class AdminCarController : Controller
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the car that was created.
        /// </summary>
        public const string CreatedCarIdTempDataKey = "AdminCreatedCarId";

        /// <summary>
        /// The route part for the controller.
        /// </summary>
        private const string CurrentControllerRoutePart = "Admin/Cars";

        /// <summary>
        /// The key for the ID of the car that was deleted. 
        /// </summary>
        public const string DeletedCarIdTempDataKey = "AdminDeletedCarId";

        /// <summary>
        /// The key to use when storing page sub titles in temporary storage.
        /// </summary>
        private const string PageSubTitleTempDataKey = "AdminCarEditPageSubTitle";

        /// <summary>
        /// The key for the deleted car redirect data stored in temp storage.
        /// </summary>
        public const string RedirectToPageAfterDeleteTempDataKey = "AdminDeletedCarRedirectToPage";

        #endregion

        #region Fields

        /// <summary>
        /// The injected car repository.
        /// </summary>
        private readonly ICarRepository _carRepository;

        /// <summary>
        /// The injected car category repository.
        /// </summary>
        private readonly ICarCategoryRepository _carCategoryRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="carRepository">The injected car repository.</param>
        /// <param name="carCategoryRepository">The injected car category repository.</param>
        public AdminCarController(ICarRepository carRepository, ICarCategoryRepository carCategoryRepository)
        {
            _carRepository = carRepository;
            this._carCategoryRepository = carCategoryRepository;
        }

        #endregion

        #region Actions

        // GET: AdminCarController/Create
        public async Task<IActionResult> Create()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Create));
            }

            CreateCarViewModel viewmodel = new CreateCarViewModel(await _carCategoryRepository.GetAllAsync());
            
            return View(viewmodel);
        }

        // POST: AdminCarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarViewModel createCarViewModel)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Create));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!DataTransferHelper.TryTransferData(createCarViewModel, out Car car))
                {
                    throw new Exception("Failed to transfer data from the view model to the entity");
                }

                var selectedCategory = await _carCategoryRepository.GetByIdAsync(createCarViewModel.SelectedCategoryId);
                car.Category = selectedCategory;

                if (createCarViewModel.UploadImages is not null && createCarViewModel.UploadImages.Count > 0)
                {
                    var savedImageFileNames = await ImageHelper.SaveUploadedImagesToDisk(createCarViewModel.UploadImages);

                    foreach (var imageFileName in savedImageFileNames)
                    {
                        car.Images.Add(new CarImage(imageFileName, car));
                    }
                }

                await _carRepository.AddAsync(car);
                TempDataHelper.Set(TempData, CreatedCarIdTempDataKey, car.CarId);
                return RedirectToAction(nameof(Details), new { id = car.CarId });
            }

            createCarViewModel.Categories = (await _carCategoryRepository.GetAllAsync()).Select(x => new CarCategoryViewModel(x)).ToList();
            return View(createCarViewModel);
        }

        // POST: AdminCarController/Delete/5
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

            var car = await _carRepository.GetByIdAsync(id);

            if (car is null)
            {
                throw new Exception($"No car found with ID '{id}'.");
            }

            if (car!.Images.Count > 0)
            {
                ImageHelper.DeleteImagesFromDisk(car!.Images.Select(x => x.FileName));
            }

            await _carRepository.DeleteAsync(id);
            TempDataHelper.Set(TempData, DeletedCarIdTempDataKey, id);

            if (TempDataHelper.TryGet(TempData, RedirectToPageAfterDeleteTempDataKey, out RedirectToActionData? data))
            {
                return RedirectToAction(data.Action, data.Controller, data.RouteValues);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: AdminCarController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Details));
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            var car = await _carRepository.GetByIdAsync(id);

            if (car is not null)
            {
                CarViewModel viewModel = new CarViewModel(car);

                if (TempDataHelper.TryGet(TempData, CreatedCarIdTempDataKey, out int carId))
                {
                    viewModel.Messages.Add(UserMesssageHelper.CreateCarCreationSuccessMessage(carId));
                }

                return View(viewModel);
            }

            throw new Exception($"Failed to show the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: AdminCarController/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Edit));
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            var car = await _carRepository.GetByIdAsync(id);

            if (car is not null)
            {
                var carCategories = await _carCategoryRepository.GetAllAsync();
                EditCarViewModel viewModel = new EditCarViewModel(car, carCategories);
                TempDataHelper.Set(TempData, PageSubTitleTempDataKey, viewModel.PageSubTitle!);
                return View(viewModel);
            }

            throw new Exception($"Failed to find the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // POST: AdminCarController/Edit/5
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCarViewModel editCarViewModel)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Edit));
            }

            if (id <= 0 || id != editCarViewModel.CarId)
            {
                throw new Exception($"Invalid ID or ID mismatch - QueryParameter: {id} - ViewModel: {editCarViewModel.CarId}");
            }

            // The images is also needed for invalid submissions, so we fetch them up here. 
            var carImages = await _carRepository.GetCarImagesAsync(id);

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!DataTransferHelper.TryTransferData(editCarViewModel, out Car car))
                {
                    throw new Exception("Failed to transfer data from view model to entity.");
                }

                var selectedCategory = await _carCategoryRepository.GetByIdAsync(editCarViewModel.SelectedCategoryId);
                car.Category = selectedCategory;

                car.Images.AddRange(carImages);

                if (editCarViewModel.UploadImages is not null && editCarViewModel.UploadImages.Count > 0)
                {
                    var savedImageFileNames = await ImageHelper.SaveUploadedImagesToDisk(editCarViewModel.UploadImages!);
                    car.Images.AddRange(savedImageFileNames.Select(x => new CarImage(x, car)));
                }

                if (editCarViewModel.DeleteImages is not null && editCarViewModel.DeleteImages.Count > 0)
                {
                    var imagesToDelete = car.Images.IntersectBy(editCarViewModel.DeleteImages, x => x.ImageId).ToList();

                    if (imagesToDelete.Count > 0)
                    {
                        ImageHelper.DeleteImagesFromDisk(imagesToDelete.Select(x => x.FileName));
                        imagesToDelete.ForEach(x => car.Images.Remove(x));
                    }
                }

                await _carRepository.UpdateAsync(car);
                var carCategories = await _carCategoryRepository.GetAllAsync();
                EditCarViewModel viewModel = new EditCarViewModel(car, carCategories);
                viewModel.Messages.Add(UserMesssageHelper.CreateCarUpdateSuccessMessage(id));
                return View(viewModel);
            }

            editCarViewModel.Images = carImages.Select(x => new CarImageViewModel(x)).ToList();

            if (TempDataHelper.TryGet(TempData, PageSubTitleTempDataKey, out string? pageSubTitle))
            {
                editCarViewModel.PageSubTitle = pageSubTitle;
                TempDataHelper.Set(TempData, PageSubTitleTempDataKey, editCarViewModel.PageSubTitle!); // The user can fail again.
            }

            return View(editCarViewModel);
        }

        // GET: AdminCarController
        public async Task<IActionResult> Index()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Index));
            }

            ListViewModel<CarViewModel> carListViewModel = new((await _carRepository.GetAllAsync()).Select(x => new CarViewModel(x)));
            SaveRedirectBackInstructionsForDeleteCarAction(nameof(Index));

            if (TempDataHelper.TryGet(TempData, DeletedCarIdTempDataKey, out int deletedCarId))
            {
                carListViewModel.Messages.Add(UserMesssageHelper.CreateCarDeletionSuccessMessage(deletedCarId));
            }

            return View(carListViewModel);
        }     

        #endregion

        #region OtherMethods

        /// <summary>
        /// Redirects to the login page and request a redirect back afterwards. 
        /// </summary>
        /// <param name="action">The action to redirect to.</param>
        /// <param name="id">An optional ID for the car.</param>
        /// <returns><see cref="IActionResult"/>.</returns>
        private IActionResult RedirectToLogin(string action, int? id = null)
        {
            RouteValueDictionary? routeValues = id is not null ? new RouteValueDictionary(new { id = id }) : null;

            TempDataHelper.Set(TempData, AdminController.RedirectToPageTempDataKey, new RedirectToActionData(
                    action, ControllerHelper.GetControllerName<AdminCarController>(), routeValues: routeValues));

            return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
        }

        /// <summary>
        /// Saves data for redirecting back to an action after a car has been deleted. 
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

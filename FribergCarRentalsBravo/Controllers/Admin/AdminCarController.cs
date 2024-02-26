﻿using FribergCarRentals.Models.Cars;
using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.Data;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.DataAccess.Repositories;
using FribergCarRentalsBravo.Helpers;
using FribergCarRentalsBravo.Models.Cars;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

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

        private readonly ICarRepository _carRepository;

        #endregion

        #region Constructors

        public AdminCarController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        #endregion

        #region Actions


        // GET: AdminCarController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminCarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarViewModel createCarViewModel)
        {
            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (!DataTransferHelper.TryTransferData(createCarViewModel, out Car car))
                {
                    throw new Exception("Failed to transfer data from the view model to the entity");
                }

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

            return View();
        }

        // POST: AdminCarController/Delete/5
        [ActionName(nameof(Delete))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var car = await _carRepository.GetByIdAsync(id);

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
                    return RedirectToAction(nameof(List));
                }
            }

            throw new Exception($"Failed to delete the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: AdminCarController/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
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
            }

            throw new Exception($"Failed to show the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // GET: AdminCarController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                var car = await _carRepository.GetByIdAsync(id);

                if (car is not null)
                {
                    EditCarViewModel viewModel = new EditCarViewModel(car);
                    TempDataHelper.Set(TempData, PageSubTitleTempDataKey, viewModel.PageSubTitle!);
                    return View(viewModel);
                }
            }

            throw new Exception($"Failed to show the car with id: {id} - ModelState.Count: {ModelState.Count} - ModelState.IsValid: {ModelState.IsValid}");
        }

        // POST: AdminCarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCarViewModel editCarViewModel)
        {
            if (id <= 0 || id != editCarViewModel.CarId)
            {
                throw new Exception($"Invalid ID or ID mismatch - QueryParameter: {id} - ViewModel: {editCarViewModel.CarId}");
            }

            // The images is also needed for invalid submissions, so we fetch them up here. 
            var carImages = await _carRepository.GetCarImagesAsync(id);

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (DataTransferHelper.TryTransferData(editCarViewModel, out Car car))
                {
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
                    EditCarViewModel viewModel = new EditCarViewModel(car);
                    viewModel.Messages.Add(UserMesssageHelper.CreateCarUpdateSuccessMessage(id));
                    return View(viewModel);
                }
            }

            editCarViewModel.Images = carImages.Select(x => new CarImageViewModel(x)).ToList();

            if (TempDataHelper.TryGet(TempData, PageSubTitleTempDataKey, out string? pageSubTitle))
            {
                editCarViewModel.PageSubTitle = pageSubTitle;
                TempDataHelper.Set(TempData, PageSubTitleTempDataKey, editCarViewModel.PageSubTitle!); // The user can fail again.
            }

            return View(editCarViewModel);
        }

        // GET: AdminCarController/List
        public async Task<IActionResult> List()
        {
            ListViewModel<CarViewModel> carListViewModel = new((await _carRepository.GetAllAsync()).Select(x => new CarViewModel(x)));
            SaveRedirectBackInstructionsForDeleteCarAction(nameof(List));

            if (TempDataHelper.TryGet(TempData, DeletedCarIdTempDataKey, out int deletedCarId))
            {
                carListViewModel.Messages.Add(UserMesssageHelper.CreateCarDeletionSuccessMessage(deletedCarId));
            }

            return View(carListViewModel);
        }
        #endregion

        #region OtherMethods

        /// <summary>
        /// Saves data for redirecting back to an action after a car has been deleted. 
        /// </summary>
        /// <param name="redirectToAction">The action to redirect to.</param>
        private void SaveRedirectBackInstructionsForDeleteCarAction(string redirectToAction)
        {
            TempDataHelper.Set(TempData, RedirectToPageAfterDeleteTempDataKey, new RedirectToActionData(
                    redirectToAction, "AdminCar"));
        }

        #endregion
    }
}
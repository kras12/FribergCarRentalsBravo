using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.Controllers.Customers;
using FribergCarRentalsBravo.Data;
using FribergCarRentalsBravo.DataAccess.Repositories;
using FribergCarRentalsBravo.Helpers;
using FribergCarRentalsBravo.Models;
using FribergCarRentalsBravo.Models.Other;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FribergCarRentalsBravo.Controllers
{
    public class HomeController : Controller
    {
        #region Fields

        ICarRepository _carRepository;

        #endregion

        #region Constructors

        public HomeController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        #endregion


        public async Task<IActionResult> Index()
        {
            List<SlideShowImageViewModel> images = new();

            var cars = (await _carRepository.GetFirstCarWithImagesByCategory()).ToList();

            foreach (var car in cars)
            {
                var image = car.Images.First();

                images.Add(new SlideShowImageViewModel(
                    ImageHelper.GetImageFileUrl(image), image.FileName, image.ImageId,
                    imageCaption: car.Category!.CategoryName,
					linksToAction: new RedirectToActionData(
                        action: nameof(CustomerOrderController.Book),
                        controller: ControllerHelper.GetControllerName<CustomerOrderController>(),
                        routeValues: new RouteValueDictionary(new { CarCategoryId = car.Category!.CarCategoryId }))));
            }

            return View(new ListViewModel<SlideShowImageViewModel>(images));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

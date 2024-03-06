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
using FribergCarRentalsBravo.Sessions;
using FribergCarRentalsBravo.Data;
using FribergCarRentalsBravo.Helpers;
using FribergCarRentals.Models.Orders;
using FribergCarRentalsBravo.Models.Admin;
using FribergCarRentals.Models.Other;
using FribergCarRentalsBravo.Models.Orders;

namespace FribergCarRentalsBravo.Controllers.Admin
{
    [Route($"Admin/Orders/[action]")]
    public class AdminOrderController : Controller
    {
        #region Constants

        /// <summary>
        /// The key for the ID of the order that was deleted.
        /// </summary>
        public const string DeletedOrderIdTempDataKey = "AdminDeletedOrderId";

        /// <summary>
        /// The key for the deleted car category redirect data stored in temp storage.
        /// </summary>
        public const string RedirectToPageAfterDeleteTempDataKey = "AdminDeletedOrderRedirectToPage";

        #endregion

        #region Fields

        private readonly IOrderRepository orderRepo;

        #endregion

        #region Constructors

        public AdminOrderController(IOrderRepository orderRepo)
        {
            this.orderRepo = orderRepo;
        }

        #endregion

        // GET: AdminOrder
        public async Task<IActionResult> Index()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Index));
            }

            ListViewModel<OrderViewModel> viewModel = new ((await orderRepo.GetAllOrdersAsync()).Select(x => new OrderViewModel(x)).ToList());

            if (TempDataHelper.TryGet(TempData, DeletedOrderIdTempDataKey, out int deletedCategoryId))
            {
                viewModel.Messages.Add(UserMesssageHelper.CreateOrderDeletionSuccessMessage(deletedCategoryId));
            }

            return View(viewModel);
        }

        // GET: AdminOrder/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if(!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Details));
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            var order = await orderRepo.GetOrderByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(new OrderViewModel(order));
        }

        // GET: AdminOrder/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Edit), id);
            }

            if (id < 0)
            {
                throw new Exception($"Invalid ID: {id}");
            }

            var order = await orderRepo.GetOrderByIdAsync(id);

            if (order is not null)
            {
                return View(new EditOrderViewModel(order));
            }

            throw new Exception($"Failed to find the order with id: {id}");
        }

        // POST: AdminOrder/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditOrderViewModel editOrderViewModel)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(Index));
            }
            
            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (editOrderViewModel.PickupDate is null || editOrderViewModel.PickupDate <= DateTime.Now)
                {
                    ModelState.AddModelError($"{nameof(EditOrderViewModel.PickupDate)}",
                        "The pickup date must be at least one day into the future.");
                }
                else if (editOrderViewModel.ReturnDate is null || editOrderViewModel.ReturnDate.Value.Date < editOrderViewModel.PickupDate.Value.Date)
                {
                    ModelState.AddModelError($"{nameof(EditOrderViewModel.ReturnDate)}",
                        "The return date can't occur before the pickup date.");
                }
                else
                {
                    var order = await orderRepo.GetOrderByIdAsync(editOrderViewModel.OrderId);

                    if (order is null)
                    {
                        throw new Exception($"Failed to retrieve order '{editOrderViewModel.OrderId}'.");
                    }

                    order.PickupDate = editOrderViewModel.PickupDate.Value;
                    order.ReturnDate = editOrderViewModel.ReturnDate.Value;
                    order.CostPerDay = editOrderViewModel.CostPerDay;
                    await orderRepo.EditOrderAsync(order);
                    EditOrderViewModel viewModel = new EditOrderViewModel();
                    viewModel.Messages.Add(UserMesssageHelper.CreateOrderUpdateSuccessMessage(editOrderViewModel.OrderId));
                    return View(viewModel);
                }
            }
            return View(editOrderViewModel);
        }

        // POST: AdminOrder/Delete/5
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

            var order = await orderRepo.GetOrderByIdAsync(id);

            if (order is null)
            {
                throw new Exception($"No order found with ID '{id}'.");
            }

            await orderRepo.DeleteOrderAsync(order);
            TempDataHelper.Set(TempData, DeletedOrderIdTempDataKey, id);

            if (TempDataHelper.TryGet(TempData, RedirectToPageAfterDeleteTempDataKey, out RedirectToActionData? data))
            {
                return RedirectToAction(data.Action, data.Controller, data.RouteValues);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }       
        }

        // GET: CustomerOrderController/PendingCars
        public IActionResult PendingCars()
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(PendingCars));
            }

            return View(new PendingCarsViewModel(havePerformedCarSearch: false));
        }

        // POST: CustomerOrderController/PendingCars
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PendingCars(PendingCarsViewModel pendingCarsViewModel)
        {
            if (!UserSessionHandler.IsAdminLoggedIn(HttpContext.Session))
            {
                return RedirectToLogin(nameof(PendingCars));
            }

            if (ModelState.Count > 0 && ModelState.IsValid)
            {
                if (pendingCarsViewModel.EndDateFilter!.Value.Date < pendingCarsViewModel.StartDateFilter!.Value.Date)
                {
                    ModelState.AddModelError($"{nameof(pendingCarsViewModel.EndDateFilter)}",
                        "The end date can't occur before the start date.");
                }
                else
                {
                    var orders = (await orderRepo.GetPendingPickups(pendingCarsViewModel.StartDateFilter.Value, pendingCarsViewModel.EndDateFilter.Value)).ToList();

                    return View(new PendingCarsViewModel(
                        havePerformedCarSearch: true,
                        orders: orders,
                        startDateFilter: pendingCarsViewModel.StartDateFilter,
                        endDateFilter: pendingCarsViewModel.EndDateFilter));
                }
            }

            return View(pendingCarsViewModel);
        }      

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
                    action, ControllerHelper.GetControllerName<AdminOrderController>(), routeValues: routeValues));

            return RedirectToAction(nameof(AdminController.Login), ControllerHelper.GetControllerName<AdminController>());
        }

        /// <summary>
        /// Saves data for redirecting back to an action after an order has been deleted. 
        /// </summary>
        /// <param name="redirectToAction">The action to redirect to.</param>
        private void SaveRedirectBackInstructionsForDeleteOrderAction(string redirectToAction)
        {
            TempDataHelper.Set(TempData, RedirectToPageAfterDeleteTempDataKey, new RedirectToActionData(
                    redirectToAction, ControllerHelper.GetControllerName<AdminCarController>()));
        }

        #endregion
    }
}

using FribergCarRentals.Models.Cars;
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
    public class AdminController : Controller
    {
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

        // POST: AdminController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {

            if (id == null || adminRep.GetAdminByIdAsync == null)
            {
                return NotFound();
            }

            var admin = await adminRep.GetAdminByIdAsync(id);

            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);

        }

        // GET: AdminController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View(await adminRep.GetAdminByIdAsync(id));
        }

        // GET: AdminController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || adminRep.GetAdminByIdAsync == null)
            {
                return NotFound();
            }

            AdminUser admin = await adminRep.GetAdminByIdAsync(id);

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
            if (id != admin.AdminId)
            {
                return NotFound();
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

        #endregion
    }
}

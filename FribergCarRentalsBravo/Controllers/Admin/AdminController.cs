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

        public async Task<IActionResult> DetailsAsync(int id)
        {
            var admin = await adminRep.GetAdminByIdAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        #endregion
    }
}

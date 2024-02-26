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

namespace FribergCarRentalsBravo.Controllers
{
    public class CarCategoryController : Controller
    {
        private readonly ICarCategory carCategoryRepo;

        public CarCategoryController(ICarCategory carCategoryRepo)
        {
            this.carCategoryRepo = carCategoryRepo;
        }

        // GET: CarCategory
        public async Task<IActionResult> Index()
        {
            return View(await carCategoryRepo.GetAllAsync());
        }

        // GET: CarCategory/Details/5
        public async Task<IActionResult> Details(int id)
        {
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
            return View();
        }

        // POST: CarCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarCategoryId,Name")] CarCategory carCategory)
        {
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
            var carCategory = await carCategoryRepo.GetByIdAsync(id);
            if (carCategory != null)
            {
                await carCategoryRepo.DeleteCarCategoryAsync(carCategory);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

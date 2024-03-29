﻿using FribergCarRentalsBravo.DataAccess.DatabaseContexts;
using FribergCarRentalsBravo.DataAccess.Entities;
using FribergCarRentalsBravo.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentalsBravo.Data
{
    public class CarCategoryRepository : ICarCategoryRepository
    {
        private ApplicationDbContext applicationDbContext;

        public CarCategoryRepository(ApplicationDbContext applicationDbContext) 
        { 
            this.applicationDbContext = applicationDbContext;
        }

        public async Task CreateNewCarCategoryAsync(CarCategory carCategory)
        {
            applicationDbContext.CarCategories.Add(carCategory);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteCarCategoryAsync(CarCategory carCategory)
        {
            applicationDbContext.Remove(carCategory);
            await applicationDbContext.SaveChangesAsync();
        }

        public Task DeleteCarCategoryByIdAsync(int id)
        {
            var car = new CarCategory() { CarCategoryId = id };
            applicationDbContext.CarCategories.Remove(car);
            return applicationDbContext.SaveChangesAsync();
        }

        public async Task<List<CarCategory>> GetAllAsync()
        {
            return await applicationDbContext.CarCategories.OrderBy(x => x.CarCategoryId).ToListAsync();
        }

        public async Task<CarCategory?> GetByIdAsync(int id)
        {
            return await applicationDbContext.CarCategories.FirstOrDefaultAsync(x => x.CarCategoryId == id);
        }

        public async Task UpdateCarCategoryAsync(CarCategory carCategory)
        {
            applicationDbContext.Attach(carCategory).State = EntityState.Modified;
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<int> GetAmountOfCarCategoriesAsync()
        {
            return applicationDbContext.CarCategories.Count();
        }
    }
}

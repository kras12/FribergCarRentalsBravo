using FribergCarRentalsBravo.DataAccess.Entities;

namespace FribergCarRentalsBravo.DataAccess.Repositories
{
    public interface ICarCategory
    {
        Task<CarCategory> GetByIdAsync(int id);
        Task<List<CarCategory>> GetAllAsync();
        Task CreateNewCarCategoryAsync(CarCategory carCategory);
        Task UpdateCarCategoryrAsync(CarCategory carCategory);
        Task DeleteCarCategoryAsync(CarCategory carCategory);
    }
}

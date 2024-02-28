using FribergCarRentalsBravo.DataAccess.Entities;

namespace FribergCarRentalsBravo.DataAccess.Repositories
{
    public interface ICarCategoryRepository
    {
        Task<CarCategory?> GetByIdAsync(int id);
        Task<List<CarCategory>> GetAllAsync();
        Task CreateNewCarCategoryAsync(CarCategory carCategory);
        Task UpdateCarCategoryrAsync(CarCategory carCategory);
        Task DeleteCarCategoryAsync(CarCategory carCategory);
    }
}

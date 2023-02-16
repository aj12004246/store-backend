using store_be.Models;

namespace store_be.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetById(int id);
        Task<bool> DeleteById(int id);
        Task CreateCategory(Category category);
        Task<Category> UpdateCategory(int id, Category categroy);

    }
}

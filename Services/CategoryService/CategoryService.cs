using store_be.Models;
namespace store_be.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }
        public Task<List<Category>> GetAllCategoriesAsync()
        {
            return _context.Categories.ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            var category =  await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new FileNotFoundException("Coupon not found");
            }
            return category;
        }


        public async Task<Category> UpdateCategory(int id, Category requested)
        {

            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName.Equals(requested.CategoryName));
            if (existingCategory != null)
            {
                throw new Exception("Category Name Taken");
            }
            
            if (existingCategory == null)
            {
                throw new FileNotFoundException("Could not update - Category does not exist");
            }
            existingCategory.CategoryName = requested.CategoryName;
            await _context.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<bool> DeleteById(int id)
        {
            var foundCategory = await _context.Categories.FindAsync(id);
            if (foundCategory == null)
            {
                return false;
            }
            _context.Categories.Remove(foundCategory);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task CreateCategory(Category category)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName.Equals(category.CategoryName));
            if (existingCategory != null)
            {
                throw new Exception("Category Name Taken");
            }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
    }
}

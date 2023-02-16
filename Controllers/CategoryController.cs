using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_be.Models;
using store_be.Services.CategoryService;
using store_be.Services.CouponService;

namespace store_be.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService categoryService)
        {
            _service = categoryService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAll()
        {
            return Ok(await _service.GetAllCategoriesAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var category = _service.GetById(id);
            if(category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteById(int id)
        {
            if (await _service.DeleteById(id))
            {
                return Ok();
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<ActionResult<List<Category>>> AddCategory(Category category)
        {
            try
            {
                await _service.CreateCategory(category);
                return Ok(await _service.GetAllCategoriesAsync());
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> UpdateCategory(int id, Category category)
        {
            return Ok(await _service.UpdateCategory(id, category));
        }
    }
}

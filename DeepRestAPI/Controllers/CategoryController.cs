using Azure;
using DeepRestAPI.Data;
using DeepRestAPI.Data.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeepRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private AppDbContext _db;
        public CategoryController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var cats = await _db.Categories.ToListAsync();
            return Ok(cats);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategories(int id)
        {
            var cat = await _db.Categories.SingleOrDefaultAsync(i => i.Id == id);
            if (cat == null)
            {
                return NotFound($"Category Id {id} Not Found");
            }
            return Ok(cat);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(string category)
        {
            Category c = new() { Name = category };
            await _db.Categories.AddAsync(c);
            _db.SaveChanges();
            return Ok(c);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            var c = await _db.Categories.SingleOrDefaultAsync(x => x.Id == category.Id);
            if(c == null)
            {
                return NotFound($"Category Id  {category.Id} Not Exsist");
            }
            c.Name = category.Name;
            _db.SaveChanges();
            return Ok(c);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCatecory(int id)
        {
            var c = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if(c == null)
            {
                return NotFound($"Category Id {id} Not Found");
            }
            _db.Categories.Remove(c);
            _db.SaveChanges();
            return Ok(c);
        }
        [HttpPatch("{id}")]

        public async Task<IActionResult> UpdateCategoryPatch([FromBody] JsonPatchDocument<Category> category,[FromRoute] int id)
        {
            var c = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (c == null)
            {
                return NotFound($"Category Id  {id} Not Exsist");
            }
            category.ApplyTo(c);
            await _db.SaveChangesAsync();
            return Ok(c);
        }
    }
}

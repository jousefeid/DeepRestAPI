using DeepRestAPI.Data;
using DeepRestAPI.Data.Models;
using DeepRestAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeepRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ItemsController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> AllItems()
        {
            var items = await _db.Items.ToListAsync();
            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> AllItems(int id)
        {
            var item = await _db.Items.SingleOrDefaultAsync(i => i.Id == id);
            if(item ==  null)
            {
                return NotFound($"Item Code {id} not exists!");
            }
            return Ok(item);
        }
        [HttpGet("ItemsWihtCategory/{idCategory}")]
        public async Task<IActionResult> AllItemsWihtCategory(int idCategory)
        {
            var item = await _db.Items.Where(i => i.CategoryId == idCategory).ToArrayAsync();
            if (item == null)
            {
                return NotFound($"Item Code {idCategory} has no items!");
            }
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> AddItem([FromForm]MdlItem mdl)
        {
            using var stream = new MemoryStream();
            await mdl.Image.CopyToAsync(stream);
            var item = new Item
            {
                Name= mdl.Name,
                Price= mdl.Price,
                Notes= mdl.Notes,
                CategoryId= mdl.CategoryId,
                Image=stream.ToArray()
            };
            await _db.Items.AddAsync(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromForm] MdlItem mdl)
        {
            var item = await _db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound($"Item with Id {id} not exists!");
            }
            var isCategoryExsists = await _db.Categories.AnyAsync(x => x.Id == mdl.CategoryId);
            if (isCategoryExsists)
            {
                return NotFound($"Category Id {id} not exists!");
            }
            if (mdl.Image != null)
            {
                using var stream = new MemoryStream();
                await mdl.Image.CopyToAsync(stream);
                item.Image=stream.ToArray();
            }
            item.Name = mdl.Name;
            item.Price = mdl.Price;
            item.Notes = mdl.Notes;
            item.CategoryId = mdl.CategoryId;
            _db.SaveChanges();
            return Ok(item);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _db.Items.SingleOrDefaultAsync(i => i.Id == id);
            if (item == null)
            {
                return NotFound($"Item with Id {id} not exists!");
            }
            _db.Items.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }

    }
}

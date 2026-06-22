using DeepRestAPI.Data;
using DeepRestAPI.Data.Models;
using DeepRestAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeepRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _db;

        public OrderController(AppDbContext db)
        {
            _db = db;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = _db.Orders.ToArray();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order != null)
            {
                DtoOrder dto = new()
                {
                    orderId = order.Id,
                    CreatedDate = order.OrderDate,
                };
                if (order.orderItems != null && order.orderItems.Any())
                {
                    foreach (var item in order.orderItems)
                    {
                        DtoOrederItem dtoitems = new()
                        {
                            itemId = item.items.Id,
                            ItemName = item.items.Name,
                            Price = item.Price,
                            Quantity = 1
                        };
                        dto.items.Add(dtoitems);
                    }
                }
                return Ok(dto);
            }
            return NotFound($"The Order Id {orderId} Not Exsist");
        }

        [HttpPost]
        public async Task<IActionResult> AddeOrder([FromBody]DtoOrder order)
        {
            if (ModelState.IsValid)
            {
                Order mdl = new()
                {
                    OrderDate = order.CreatedDate,
                    orderItems = new List<OrderItem>()
                };
                foreach (var item in order.items)
                {
                    OrderItem orderItem = new()
                    {
                        ItemId = item.itemId,
                        Price = item.Price
                    };
                    mdl.orderItems.Add(orderItem);
                }
                await _db.Orders.AddAsync(mdl);
                await _db.SaveChangesAsync();
                order.orderId = mdl.Id;
                return Ok(order);
            }
            return BadRequest();
        }

    }
}

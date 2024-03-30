using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoppingSiteAPI.Models;

namespace SoppingSiteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly APIContext _dbcontext;

        public OrderController(APIContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            if (_dbcontext == null)
            {
                return NotFound();
            }
            return await _dbcontext.Orders.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            if (_dbcontext == null)
            {
                return NotFound();
            }

            var order = await _dbcontext.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _dbcontext.Orders.Add(order);
            await _dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(PostOrder), new { id = order.OrderId }, order);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }
            _dbcontext.Entry(order).State = EntityState.Modified;

            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_dbcontext.Orders == null)
            {
                return NotFound();
            }

            var order = await _dbcontext.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            _dbcontext.Orders.Remove(order);

            await _dbcontext.SaveChangesAsync();

            return Ok();
        }

        private bool OrderAvailable(int id)
        {
            return (_dbcontext.Orders?.Any(x => x.OrderId == id)).GetValueOrDefault();
        }
    }
}

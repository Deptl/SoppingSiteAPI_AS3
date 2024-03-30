using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoppingSiteAPI.Models;

namespace SoppingSiteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly APIContext _dbcontext;

        public CartController(APIContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            if (_dbcontext == null)
            {
                return NotFound();
            }
            return await _dbcontext.Carts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            if (_dbcontext == null)
            {
                return NotFound();
            }

            var cart = await _dbcontext.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return cart;
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            _dbcontext.Carts.Add(cart);
            await _dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(PostCart), new { id = cart.CartId }, cart);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCart(int id, Cart cart)
        {
            if (id != cart.CartId)
            {
                return BadRequest();
            }
            _dbcontext.Entry(cart).State = EntityState.Modified;

            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartAvailable(id))
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
        public async Task<IActionResult> DeleteCart(int id)
        {
            if (_dbcontext.Carts == null)
            {
                return NotFound();
            }

            var cart = await _dbcontext.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            _dbcontext.Carts.Remove(cart);

            await _dbcontext.SaveChangesAsync();

            return Ok();
        }

        private bool CartAvailable(int id)
        {
            return (_dbcontext.Carts?.Any(x => x.CartId == id)).GetValueOrDefault();
        }
    }
}

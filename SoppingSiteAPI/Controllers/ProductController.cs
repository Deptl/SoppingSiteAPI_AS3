using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoppingSiteAPI.Models;

namespace SoppingSiteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly APIContext _dbcontext;

        public ProductController(APIContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if (_dbcontext == null)
            {
                return NotFound();
            }
            return await _dbcontext.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (_dbcontext == null)
            {
                return NotFound();
            }

            var product = await _dbcontext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _dbcontext.Products.Add(product);
            await _dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(PostProduct), new { id = product.ProductId }, product);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }
            _dbcontext.Entry(product).State = EntityState.Modified;

            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductAvailable(id))
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
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_dbcontext.Products == null)
            {
                return NotFound();
            }

            var product = await _dbcontext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _dbcontext.Products.Remove(product);

            await _dbcontext.SaveChangesAsync();

            return Ok();
        }

        private bool ProductAvailable(int id)
        {
            return (_dbcontext.Products?.Any(x => x.ProductId == id)).GetValueOrDefault();
        }
    }
}

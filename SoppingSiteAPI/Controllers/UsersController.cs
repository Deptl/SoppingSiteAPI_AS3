using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoppingSiteAPI.Models;

namespace SoppingSiteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly APIContext _dbcontext;

        public UsersController(APIContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            if (_dbcontext == null)
            {
                return NotFound();
            }
            return await _dbcontext.Persons.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            if (_dbcontext == null)
            {
                return NotFound();
            }

            var user = await _dbcontext.Persons.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public async Task<ActionResult<Users>> PostUser(Users users)
        {
            _dbcontext.Persons.Add(users);
            await _dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(PostUser), new { id = users.UserId }, users);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, Users users)
        {
            if (id != users.UserId)
            {
                return BadRequest();
            }
            _dbcontext.Entry(users).State = EntityState.Modified;

            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAvailable(id))
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
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_dbcontext.Persons == null)
            {
                return NotFound();
            }

            var user = await _dbcontext.Persons.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _dbcontext.Persons.Remove(user);

            await _dbcontext.SaveChangesAsync();

            return Ok();
        }

        private bool UserAvailable(int id)
        {
            return (_dbcontext.Persons?.Any(x => x.UserId == id)).GetValueOrDefault();
        }
    }
}

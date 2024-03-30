using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoppingSiteAPI.Models;

namespace SoppingSiteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly APIContext _dbcontext;

        public CommentController(APIContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            if (_dbcontext == null)
            {
                return NotFound();
            }
            return await _dbcontext.Comments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            if (_dbcontext == null)
            {
                return NotFound();
            }

            var cmt = await _dbcontext.Comments.FindAsync(id);
            if (cmt == null)
            {
                return NotFound();
            }
            return cmt;
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment cmt)
        {
            _dbcontext.Comments.Add(cmt);
            await _dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(PostComment), new { id = cmt.CommentId }, cmt);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment(int id, Comment cmt)
        {
            if (id != cmt.CommentId)
            {
                return BadRequest();
            }
            _dbcontext.Entry(cmt).State = EntityState.Modified;

            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CmtAvailable(id))
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
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (_dbcontext.Comments == null)
            {
                return NotFound();
            }

            var cmt = await _dbcontext.Comments.FindAsync(id);

            if (cmt == null)
            {
                return NotFound();
            }

            _dbcontext.Comments.Remove(cmt);

            await _dbcontext.SaveChangesAsync();

            return Ok();
        }

        private bool CmtAvailable(int id)
        {
            return (_dbcontext.Comments?.Any(x => x.CommentId == id)).GetValueOrDefault();
        }
    }
}

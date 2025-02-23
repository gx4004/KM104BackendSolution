using Microsoft.AspNetCore.Mvc;
using Blog.API.Data;
using Blog.API.Models.Entities;
using System.Linq;

namespace Blog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly BlogDbContext _context;

        public CommentController(BlogDbContext context)
        {
            _context = context;
        }

        // GET: api/Comment?postId=1
        [HttpGet]
        public IActionResult GetComments([FromQuery] int postId)
        {
            var comments = _context.Comments
                .Where(c => c.BlogPostId == postId)
                .ToList();
            return Ok(comments);
        }

        // POST: api/Comment
        [HttpPost]
        public IActionResult CreateComment([FromBody] Comment comment)
        {
            if (comment == null)
                return BadRequest("Comment data is null.");

            // İstersen BlogPost var mı diye kontrol:
            var post = _context.BlogPosts.Find(comment.BlogPostId);
            if (post == null)
                return NotFound("Related BlogPost not found.");

            _context.Comments.Add(comment);
            _context.SaveChanges();
            return Ok(comment);
        }

        // PUT: api/Comment/5
        [HttpPut("{id}")]
        public IActionResult UpdateComment(int id, [FromBody] Comment updated)
        {
            var existing = _context.Comments.Find(id);
            if (existing == null)
                return NotFound("Comment not found.");

            existing.Content = updated.Content;
            existing.IsApproved = updated.IsApproved;
            _context.SaveChanges();
            return Ok(existing);
        }

        // DELETE: api/Comment/5
        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment == null)
                return NotFound("Comment not found.");

            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return Ok("Comment deleted.");
        }
    }
}

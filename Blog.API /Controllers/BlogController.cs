using Microsoft.AspNetCore.Mvc;
using Blog.API.Data;
using Blog.API.Models.Entities;
using System.Linq;

namespace Blog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BlogDbContext _context;

        public BlogController(BlogDbContext context)
        {
            _context = context;
        }

        // GET: api/Blog
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            var posts = _context.BlogPosts.ToList();
            return Ok(posts);
        }

        // POST: api/Blog
        [HttpPost]
        public IActionResult CreatePost([FromBody] BlogPost post)
        {
            if (post == null)
                return BadRequest("Post data is null.");

            _context.BlogPosts.Add(post);
            _context.SaveChanges();
            return Ok(post);
        }

        // PUT: api/Blog/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePost(int id, [FromBody] BlogPost updated)
        {
            var post = _context.BlogPosts.FirstOrDefault(p => p.Id == id);
            if (post == null)
                return NotFound("Post not found.");

            post.Title = updated.Title;
            post.Content = updated.Content;
            _context.SaveChanges();
            return Ok(post);
        }

        // DELETE: api/Blog/{id}
        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            var post = _context.BlogPosts.FirstOrDefault(p => p.Id == id);
            if (post == null)
                return NotFound("Post not found.");

            _context.BlogPosts.Remove(post);
            _context.SaveChanges();
            return Ok("Post deleted.");
        }
    }
}
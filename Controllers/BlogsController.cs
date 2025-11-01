using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApi.Data;
using BlogApi.Models;

namespace BlogApi.Controllers
{
    [Authorize] // Protect all endpoints
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BlogsController> _logger;

        public BlogsController(AppDbContext context, ILogger<BlogsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //  GET with Pagination
        [HttpGet]
        [AllowAnonymous] // public list allowed (optional)
        public async Task<IActionResult> GetBlogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page <= 0) page = 1;
                if (pageSize <= 0) pageSize = 10;

                var totalCount = await _context.Blogs.CountAsync();
                var blogs = await _context.Blogs
                    .OrderByDescending(b => b.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    data = blogs,
                    totalCount,
                    currentPage = page,
                    pageSize,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching blogs");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        //  GET single blog
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
                return NotFound(new { message = "Blog not found" });

            return Ok(blog);
        }

        //  CREATE blog (authenticated)
        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] Blog blog)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid data" });

            blog.CreatedAt = DateTime.UtcNow;
            blog.UpdatedAt = DateTime.UtcNow;

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBlog), new { id = blog.Id }, blog);
        }

        //  UPDATE blog
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] Blog updatedBlog)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
                return NotFound(new { message = "Blog not found" });

            blog.Title = updatedBlog.Title;
            blog.Content = updatedBlog.Content;
            blog.Author = updatedBlog.Author;
            blog.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(blog);
        }

        //  DELETE blog
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
                return NotFound(new { message = "Blog not found" });

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Blog deleted successfully" });
        }
    }
}

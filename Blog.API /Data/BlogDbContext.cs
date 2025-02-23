using Microsoft.EntityFrameworkCore;
using Blog.API.Models.Entities;

namespace Blog.API.Data
{
    public class BlogDbContext : DbContext
    {
        // DİKKAT: ": DbContext" ifadesi şart
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
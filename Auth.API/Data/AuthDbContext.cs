using Microsoft.EntityFrameworkCore;               // EF Core namespace
using Auth.API.Models.Entities;                    // User sınıfının olduğu namespace

namespace Auth.API.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}